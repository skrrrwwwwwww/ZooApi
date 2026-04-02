namespace ZooApi.Application.Services;

public class OwnerCreatedConsumer(
    ILogger<OwnerCreatedConsumer> logger, 
    IRedisCacheService cache,
    IEmailService emailService, 
    IConfiguration configuration) 
    : IConsumer<OwnerCreated> 
{
    public async Task Consume(ConsumeContext<OwnerCreated> context)
    {
        var message = context.Message;
        
        logger.LogInformation("Регистрация владельца: [ID: {Id}, Name: {Name}]", message.Id, message.Name);

        // Используем твой CacheKeys вместо "owner_"
        var cacheKey = CacheKeys.GetOwnerKey(message.Id);
        await cache.SetAsync(cacheKey, message, TimeSpan.FromMinutes(30));
        
        logger.LogInformation("Владелец {Id} успешно добавлен в кэш.", message.Id);
        
        var recipientEmail = configuration["MailSettings:Username"];
    
        if (!string.IsNullOrEmpty(recipientEmail))
        {
            // Используем интерполяцию строк (как в Animal) для единообразия
            var emailBody = $"<h1>Новый партнер!</h1><p>Владелец <b>{message.Name}</b> (ID: {message.Id}) добавлен.</p>";
            
            await emailService.SendEmailAsync(recipientEmail, "ZooApi: Регистрация владельца", emailBody);
            logger.LogInformation("Email ушел на {Email}", recipientEmail);
        }
        else logger.LogWarning("Email не найден в конфигах.");
    }
}