using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ZooApi.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    // Свежая БД без миграций падает с 42P01 уже на OutboxState — первой таблице,
    // которую пишет publish-путь. Поэтому схему доводим до актуальной при старте.
    public static async Task MigrateDatabaseAsync(
        this IHost host, int maxAttempts = 10, TimeSpan? retryDelay = null)
    {
        using var scope = host.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ZooDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ZooDbContext>>();
        var delay = retryDelay ?? TimeSpan.FromSeconds(3);

        for (var attempt = 1; ; attempt++)
        {
            try
            {
                var pending = (await context.Database.GetPendingMigrationsAsync()).ToArray();

                if (pending.Length == 0)
                {
                    logger.LogInformation("Схема БД актуальна, миграции не требуются.");
                    return;
                }

                logger.LogInformation(
                    "Применяю миграции ({Count}): {Migrations}", pending.Length, string.Join(", ", pending));

                await context.Database.MigrateAsync();
                return;
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                logger.LogWarning(
                    "Не удалось применить миграции (попытка {Attempt}/{MaxAttempts}): {Message}",
                    attempt, maxAttempts, ex.Message);

                await Task.Delay(delay);
            }
        }
    }
}
