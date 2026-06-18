global using Tusk.Domain;
global using Tusk.Application;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Mapster;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Text.Json.Serialization;
using DispatchR.Extensions;
using Tusk.Api.Filters;
using Tusk.Api.Health;
using Tusk.Api.Infrastructure;
using Tusk.Api.Persistence;
using Tusk.Application.Persistence;

namespace Tusk.Api;

public class Startup(
    IConfiguration configuration,
    IWebHostEnvironment env)
{
    public IConfiguration Configuration { get; } = configuration;

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public void ConfigureServices(IServiceCollection services)
    {
        // Add DispatchR - must be first
        services.AddDispatchR(typeof(ITuskDbContext).Assembly, withPipelines: true, withNotifications: true);

#if (!DisableAuthentication)
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["jwt:issuer"],
                    ValidAudience = Configuration["jwt:issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"]!))
                });

        // Optional: At least a module claim is required to use any protected endpoint
        services.AddAuthorization(
            auth => auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireClaim("modules", "claim-module-name")
                .Build());
#endif

        services.AddCors(options =>
            options.AddPolicy("Locations",
                builder =>
                {
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    if (env.IsDevelopment())
                    {
                        builder.AllowAnyOrigin();
                    }
                    else
                    {
                        builder.WithOrigins("http://localhost:4200"); //TODO add production origins
                    }
                }));

        services.AddHttpContextAccessor();
        services.AddApiDocumentation();

        // Default Health Checks
        services.AddHealthChecks()
            //.AddSqlServer(EnvFactory.GetConnectionString()) //TODO Enable if real MSSQL-Server is given
            .AddCheck<ApiHealthCheck>("api");

        services.AddScoped<ITuskDbContext, TuskDbContext>(sp =>
            new TuskDbContext(env.EnvironmentName, sp.GetService<IGetClaimsProvider>()));

        TypeAdapterConfig.GlobalSettings.Scan(typeof(ITuskDbContext).Assembly);
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);

        // Optional: Avoid the MultiPartBodyLength error
        services.Configure<FormOptions>(o =>
        {
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = int.MaxValue;
            o.MemoryBufferThreshold = int.MaxValue;
        });

        services.AddValidatorsFromAssemblyContaining<ITuskDbContext>(ServiceLifetime.Transient);

        // Add my own services here
        services.AddScoped<IGetClaimsProvider, GetClaimsFromUser>();
        services.AddSingleton<IDateTime, MachineDateTime>();

        services.AddControllers(options => options.Filters.Add<CustomExceptionFilter>());
        services.ConfigureHttpJsonOptions(o =>
            o.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public void Configure(
        IApplicationBuilder app)
    {
        app.UseCors("Locations");

        app.UseHealthChecks("/api/health", new HealthCheckOptions { ResponseWriter = WriteHealthCheckResponse });

        app.UseRouting();
#if (!DisableAuthentication)
        app.UseAuthentication();
        app.UseAuthorization();
#endif
        app.UseEndpoints(endpoints =>
        {
            endpoints.UseApiDocumentation();
            endpoints.MapControllers();
        });
    }

    private static readonly JsonSerializerOptions IndentedJsonOptions = new() { WriteIndented = true };

    private static Task WriteHealthCheckResponse(
        HttpContext httpContext,
        HealthReport result)
    {
        httpContext.Response.ContentType = "application/json";
        var payload = new
        {
            status = result.Status.ToString(),
            results = result.Entries.ToDictionary(
                pair => pair.Key,
                pair => new
                {
                    status = pair.Value.Status.ToString(),
                    exception = pair.Value.Exception?.Message,
                    description = pair.Value.Description,
                    data = pair.Value.Data
                })
        };
        return httpContext.Response.WriteAsync(
            JsonSerializer.Serialize(payload, IndentedJsonOptions)
        );
    }
}
