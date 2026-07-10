using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Server.Application.Configurations;
using Server.Application.Interfaces.Service;

namespace Server.Infrastructure.Services;

public class EmailService(IOptions<EmailConfigurations> emailConfigurations) : IEmailService
{
    private readonly EmailConfigurations _emailConfigurations = emailConfigurations.Value;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(_emailConfigurations.Name, _emailConfigurations.Email));

        message.To.Add(
            MailboxAddress.Parse(to));

        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = body
        };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _emailConfigurations.Host,
            _emailConfigurations.Port,
            SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _emailConfigurations.Email,
            _emailConfigurations.Password);

        await smtp.SendAsync(message);

        await smtp.DisconnectAsync(true);
    }
}