using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;

namespace cusho.Configuration.Extensions;

public static class DocumentationExtensions
{
    public static IHostApplicationBuilder AddDocumentation(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(options =>
        {
            options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;

            options.AddDocumentTransformer((document, context, token) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "Cusho API",
                    Version = "v1",
                    Description = "Cusho Backend API"
                };
                return Task.CompletedTask;
            });
        });

        return builder;
    }

    public static IApplicationBuilder UseSwaggerWithDefaults(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "Cusho API v1");
                options.RoutePrefix = "swagger";
                options.DisplayRequestDuration();
                options.EnableTryItOutByDefault();
            });
        }

        return app;
    }
}
