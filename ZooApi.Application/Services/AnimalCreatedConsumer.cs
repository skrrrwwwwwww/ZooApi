namespace ZooApi.Application.Services;

public class AnimalCreatedConsumer(ILogger<AnimalCreatedConsumer> logger, 
                                   IRedisCacheService cache,
                                   IEmailService emailService, 
                                   IConfiguration configuration) 
                                   : IConsumer<AnimalCreated> 
{
    public async Task Consume(ConsumeContext<AnimalCreated> context)
    {
        var message = context.Message;
        
        logger.LogInformation($"Получено новое животное: (ID: {message.Id}) Имя: {message.Name}, порода: {message.Species} ");

        var cacheKey = CacheKeys.GetAnimalKey(message.Id);
        
        await cache.SetAsync(cacheKey, message, TimeSpan.FromMinutes(10));
        
        logger.LogInformation("Животное (ID: {Id}) успешно добавлено в Redis кэш.", message.Id);
        
        var recipientEmail = configuration["MailSettings:Username"];
    
        if (!string.IsNullOrEmpty(recipientEmail))
        {
            var emailBody = $"<h1>Привет!</h1><p>Животное {message.Name} породы {message.Species} (ID: {message.Id}) добавлено.</p>";
            await emailService.SendEmailAsync(recipientEmail, "Уведомление: Новое животное создано", emailBody);
            logger.LogInformation("Уведомление отправлено на {Email}", recipientEmail);
        }
        else logger.LogWarning("Не удалось отправить уведомление, адрес получателя не найден в настройках.");
    }
}