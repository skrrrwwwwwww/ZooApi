namespace ZooApi.Web.MiddlewareExtensions;

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
                    Name = "Техподдержка Zoo API",
                    Email = "Shikarevivan2004@gmail.com"
                };
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