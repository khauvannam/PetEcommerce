using MimeKit;

namespace Shared.Shared;

public class Message
{
    private MimeMessage Messages { get; } = new();

    public MailboxAddress CreateAddress(string address, string username) => new(username, address);

    public MimeMessage CreateMessage(MailboxAddress address, string subject, MimeEntity body)
    {
        Messages.To.Add(address);
        Messages.Subject = subject;
        Messages.Body = body;
        return Messages;
    }
}
