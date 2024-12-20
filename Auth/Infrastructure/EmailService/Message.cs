using MimeKit;

namespace Infrastructure.EmailService;

public class Message
{
    public MailboxAddress To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }

    public Message(string to, string subject, string content)
    {
        To = MailboxAddress.Parse(to);
        Subject = subject;
        Content = content;
    }
}
