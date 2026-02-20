namespace ZooApi.Application.Services;

public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();
        var fromEmail = configuration["MailSettings:Username"] ?? "zoo@api.com";
        email.From.Add(MailboxAddress.Parse(fromEmail));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();

        // 1. Читаем конфиги
        var host = configuration["MailSettings:Host"] ?? "mailhog";
        var portString = configuration["MailSettings:Port"];
        var port = string.IsNullOrEmpty(portString) ? 1025 : int.Parse(portString);

        // 2. ПРОВЕРКА-ПРЕДОХРАНИТЕЛЬ: если подтянулся Gmail или localhost — меняем на MailHog
        if (host.Contains("gmail") || host == "localhost" || string.IsNullOrEmpty(host))
        {
            host = "mailhog";
            port = 1025;
        }

        logger.LogInformation("[SMTP DEBUG] Попытка подключения к {Host}:{Port}", host, port);

        try 
        {
            smtp.CheckCertificateRevocation = false;
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            // 3. Подключаемся БЕЗ STARTTLS (None)
            await smtp.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.None);

            // 4. Аутентификация только если это НЕ mailhog (он её не просит)
            var password = configuration["MailSettings:Password"];
            if (host != "mailhog" && !string.IsNullOrEmpty(password))
            {
                await smtp.AuthenticateAsync(configuration["MailSettings:Username"], password);
            }

            await smtp.SendAsync(email);
            logger.LogInformation("[SMTP SUCCESS] Письмо успешно отправлено на {Email} через {Host}", toEmail, host);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[SMTP ERROR] Ошибка при отправке письма через {Host}", host);
            throw; // Outbox сделает Retry
        }
        finally
        {
            if (smtp.IsConnected)
                await smtp.DisconnectAsync(true);
        }
    }
}