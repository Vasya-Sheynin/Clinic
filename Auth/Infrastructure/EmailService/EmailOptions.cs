namespace Infrastructure.EmailService;

public class EmailOptions
{
    public string From { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
}
