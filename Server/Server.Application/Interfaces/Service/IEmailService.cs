namespace Server.Application.Interfaces.Service;

public interface IEmailService
{
    Task SendEmailAsync(string to, string sub, string body);
}