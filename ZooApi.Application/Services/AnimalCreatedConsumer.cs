    using MassTransit;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ZooApi.Application.Common;
    using ZooApi.Application.Common.Contracts;
    using ZooApi.Application.Interfaces;

    namespace ZooApi.Application.Services;

    public class AnimalCreatedConsumer : IConsumer<AnimalCreated>
    {
        private readonly ILogger<AnimalCreatedConsumer> _logger;
        private readonly IRedisCacheService _cache;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AnimalCreatedConsumer(ILogger<AnimalCreatedConsumer> logger, IRedisCacheService cache, IEmailService emailService, IConfiguration configuration)
        {
            _logger = logger;
            _cache = cache;
            _emailService = emailService;
            _configuration = configuration;
        }
        
        public async Task Consume(ConsumeContext<AnimalCreated> context)
        {
            var message = context.Message;
            
            _logger.LogInformation($"Получено новое животное: (ID: {message.Id}) Имя: {message.Name}, порода: {message.Species} ");

            var cacheKey = CacheKeys.GetAnimalKey(message.Id);
            
            await _cache.SetAsync(cacheKey, message, TimeSpan.FromMinutes(10));
            
            _logger.LogInformation("Животное (ID: {Id}) успешно добавлено в Redis кэш.", message.Id);
            
            // Получаем адрес почты из appsettings, чтобы отправить письмо себе же
            var recipientEmail = _configuration["MailSettings:Username"];
        
            if (!string.IsNullOrEmpty(recipientEmail))
            {
                var emailBody = $"<h1>Привет!</h1><p>Животное {message.Name} породы {message.Species} (ID: {message.Id}) добавлено.</p>";
                await _emailService.SendEmailAsync(recipientEmail, "Уведомление: Новое животное создано", emailBody);
                _logger.LogInformation("Уведомление отправлено на {Email}", recipientEmail);
            }
            else
            {
                _logger.LogWarning("Не удалось отправить уведомление, адрес получателя не найден в настройках.");
            }
        }
    }