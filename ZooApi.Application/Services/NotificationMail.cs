using Microsoft.Extensions.Logging;

namespace ZooApi.Application.Services;

internal static class NotificationMail
{
    // Письмо — это побочный эффект, и бросать из него исключение нельзя:
    // консьюмер под ретраями (UseMessageRetry) тогда переигрывается целиком,
    // и одно и то же письмо уходит четыре раза. Сбой SMTP не должен
    // откатывать обработку доменного события.
    public static async Task SendAsync(
        ILogger logger,
        IEmailService emailService,
        string recipientEmail,
        string subject,
        string body)
    {
        try
        {
            await emailService.SendEmailAsync(recipientEmail, subject, body);
            logger.LogInformation("Email ушел на {Email}", recipientEmail);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Не удалось отправить уведомление на {Email}. Событие обработано, "
                + "повторная отправка не выполняется: {Reason}", recipientEmail, ex.Message);
        }
    }
}
