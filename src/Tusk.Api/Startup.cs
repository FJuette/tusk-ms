using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tusk.Api.Filters;
using Tusk.Api.Health;
using Tusk.Api.Infrastructure;
using Tusk.Api.Infrastructure.Behaviours;
using Tusk.Api.Persistence;

namespace Tusk.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration,
            IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public void ConfigureServices(
            IServiceCollection services)
        {
            // Add MediatR - must be first
            services.AddMediatR(Assembly.GetExecutingAssembly());

#if (!DisableAuthentication)
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = EnvFactory.GetJwtIssuer(),
                        ValidAudience = EnvFactory.GetJwtIssuer(),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvFactory.GetJwtKey()))
                    });

            // At least a module claim is required to use any protected endpoint
            services.AddAuthorization(
                auth => auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireClaim("modules", "claim-module-name")
                    .Build());
#endif

            services.AddCors(options =>
                options.AddPolicy("Locations",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200");
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.AllowAnyOrigin(); //TODO remove in production and add to origin list
                    }));

            // Add Swagger
            services.AddSwaggerDocumentation();

            // Add Health Checks
            services.AddHealthChecks()
                //.AddSqlServer(EnvFactory.GetConnectionString()) //TODO Enable if real MSSQL-Server is given
                .AddCheck<ApiHealthCheck>("api");

            services.AddScoped<TuskDbContext>(sp => new TuskDbContext(_env.EnvironmentName, sp.GetService<IGetClaimsProvider>()));

            services.AddAutoMapper(typeof(Startup));

            // Avoid the MultiPartBodyLength error
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            // Add my own services here
            services.AddScoped<IGetClaimsProvider, GetClaimsFromUser>();
            services.AddSingleton<IDateTime, MachineDateTime>();

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(EventLoggerBehavior<,>));

            services.AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilter)))
                .AddFluentValidation(fv =>
                {
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                }).AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public void Configure(
            IApplicationBuilder app)
        {
            app.UseCors("Locations");
            app.UseSwaggerDocumentation();

            app.UseHealthChecks("/health", new HealthCheckOptions { ResponseWriter = WriteHealthCheckResponse });

            app.UseRouting();
#if (!DisableAuthentication)
            app.UseAuthentication();
            app.UseAuthorization();
#endif
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static Task WriteHealthCheckResponse(
            HttpContext httpContext,
            HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(
                    result.Entries.Select(pair =>
                        new JProperty(pair.Key, new JObject(
                            new JProperty("status", pair.Value.Status.ToString()),
                            new JProperty("exception", pair.Value.Exception?.Message),
                            new JProperty("description", pair.Value.Description),
                            new JProperty("data", new JObject(pair.Value.Data.Select(
                                p => new JProperty(p.Key, p.Value)))
                            )
                        )))
                ))
            );
            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented)
            );
        }
    }
}
