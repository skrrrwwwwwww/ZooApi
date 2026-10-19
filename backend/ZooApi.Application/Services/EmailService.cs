namespace ZooApi.Application.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var host = configuration["MailSettings:Host"];
        var username = configuration["MailSettings:Username"];
        var password = configuration["MailSettings:Password"];
        if (string.IsNullOrWhiteSpace(host)
            || string.IsNullOrWhiteSpace(username)
            || string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "MailSettings:Host, MailSettings:Username, and MailSettings:Password must be configured.");
        }

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(username));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();

        var port = int.TryParse(configuration["MailSettings:Port"], out var configuredPort)
            ? configuredPort
            : 587;

        await smtp.ConnectAsync(
            host,
            port,
            SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(username, password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}