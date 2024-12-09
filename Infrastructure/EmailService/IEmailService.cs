namespace Infrastructure.EmailService;

public interface IEmailService
{
    public Task SendEmail(Message message);
}
