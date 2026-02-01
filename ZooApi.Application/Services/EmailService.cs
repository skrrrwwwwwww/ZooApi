using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using ZooApi.Application.Interfaces;

namespace ZooApi.Application.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["MailSettings:Username"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
        
        using var smtp = new SmtpClient();
        
        var port = int.TryParse(_configuration["MailSettings:Port"], out var p) ? p : 587;
        
        await smtp.ConnectAsync(
            _configuration["MailSettings:Host"],
            int.Parse(_configuration["MailSettings:Port"]), 
            SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_configuration["MailSettings:Username"], _configuration["MailSettings:Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}