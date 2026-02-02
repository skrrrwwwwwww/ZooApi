using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using ZooApi.Application.Interfaces;

namespace ZooApi.Application.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(configuration["MailSettings:Username"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
        
        using var smtp = new SmtpClient();
        
        var port = int.TryParse(configuration["MailSettings:Port"], out var p) ? p : 587;
        
        await smtp.ConnectAsync(
            configuration["MailSettings:Host"],
            int.Parse(configuration["MailSettings:Port"]), 
            SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(configuration["MailSettings:Username"], configuration["MailSettings:Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}