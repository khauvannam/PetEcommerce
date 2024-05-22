using MimeKit;

namespace Shared.Entities.Services;

public static class Message
{
    public static MailboxAddress CreateAddress(string address, string username) =>
        new(username, address);

    public static MimeMessage CreateMessage(MailboxAddress address, string subject, MimeEntity body)
    {
        return new MimeMessage
        {
            To = { address },
            Subject = subject,
            Body = body
        };
    }
}
