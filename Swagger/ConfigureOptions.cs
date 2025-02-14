using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace APIMain.Swagger {
    /// <summary>
    /// Sets up an authentication window inside Swagger.
    /// </summary>
    /// <remarks>
    /// <para>Normally Swagger just shows endpoints without possibility to authenticate.</para>
    /// </remarks>
    public class ConfigureOptions : IConfigureOptions<SwaggerGenOptions> {
        public void Configure(SwaggerGenOptions options) {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                In = ParameterLocation.Header,
                Description = "Enter the received token here",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }, Array.Empty<string>()
                }
            });
        }
    }
}
