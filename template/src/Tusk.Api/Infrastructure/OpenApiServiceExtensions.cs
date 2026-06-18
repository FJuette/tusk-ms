using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace Tusk.Api.Infrastructure;

public static class OpenApiServiceExtensions
{
    public static IServiceCollection AddApiDocumentation(
        this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
#if (!DisableAuthentication)
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                document.Security ??= [];
                document.Security.Add(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecuritySchemeReference("Bearer", document), [] }
                });
                return Task.CompletedTask;
            });
#endif
        });

        return services;
    }

    public static IEndpointRouteBuilder UseApiDocumentation(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapOpenApi();
        endpoints.MapScalarApiReference();
        return endpoints;
    }
}
