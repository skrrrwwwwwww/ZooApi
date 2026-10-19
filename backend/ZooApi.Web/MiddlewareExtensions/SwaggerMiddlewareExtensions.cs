namespace ZooApi.Web.MiddlewareExtensions;

using Microsoft.OpenApi;

public static class SwaggerMiddlewareExtensions
{
    public static void AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info.Title = "Zoo Management API";
                document.Info.Version = "v1";
                document.Info.Description = @"
### 🐾 Система управления современным зоопарком

Это API предоставляет полный инструментарий для работы с обитателями и их владельцами.

**Основные возможности:**
*   **Animals**: Полный цикл учета животных — от регистрации нового жильца до выписки. Можно кормить питомцев и играть с ними через соответствующие интерактивные эндпоинты.
*   **Owners**: Управление базой владельцев (спонсоров/опекунов) с поддержкой пагинации для больших списков.

---
*По вопросам интеграции обращайтесь в отдел разработки ZooApi.*";
                document.Info.Contact = new Microsoft.OpenApi.OpenApiContact
                {
                    Name = "Zoo API Support"
                };

                var components = document.Components ??= new OpenApiComponents();
                components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                if (document.Paths is not { } paths)
                    return Task.CompletedTask;

                foreach (var (path, pathItem) in paths)
                {
                    if (path == "/health")
                        continue;

                    if (pathItem.Operations is null)
                        continue;

                    foreach (var operation in pathItem.Operations.Values)
                    {
                        operation.Security ??= [];
                        operation.Security.Add(new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                        });
                    }
                }

                return Task.CompletedTask;
            });
        });
    }

    // UI рендерит тот же документ, что отдаёт MapOpenApi() на /openapi/v1.json,
    // поэтому второй генератор документа (AddSwaggerGen) не нужен.
    public static void UseSwaggerDocumentation(this WebApplication app)
    {
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "ZooApi v1");
            options.DocumentTitle = "Zoo Management API";
        });
    }
}