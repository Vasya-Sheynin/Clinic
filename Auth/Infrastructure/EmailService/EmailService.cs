using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.EmailService;

public class EmailService : IEmailService
{
    private readonly IOptions<EmailOptions> _emailOptions;

    public EmailService(IOptions<EmailOptions> options)
    {
        _emailOptions = options;
    }

    public async Task SendEmail(Message message)
    {
        var emailMessage = CreateEmailMessage(message);
        await Send(emailMessage);
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(MailboxAddress.Parse(_emailOptions.Value.From));
        emailMessage.To.Add(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
        return emailMessage;
    }

    private async Task Send(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(_emailOptions.Value.SmtpServer, _emailOptions.Value.Port, true);
        client.AuthenticationMechanisms.Remove("XOAUTH2");
        await client.AuthenticateAsync(Environment.GetEnvironmentVariable("SMTP_USER"), Environment.GetEnvironmentVariable("SMTP_PASS"));
        await client.SendAsync(mailMessage);
        await client.DisconnectAsync(true);
    }
}
