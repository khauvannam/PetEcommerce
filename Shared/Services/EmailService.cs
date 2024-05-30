using MailKit.Net.Smtp;
using MimeKit;
using Shared.Domain.Services;

namespace Shared.Services;

public static class EmailService
{
    public static async Task SendEmailAsync(MimeMessage message)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, true);
        await client.AuthenticateAsync("singgaporestore220803@gmail.com", "");
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public static MimeMessage CreateEmail(
        string email,
        string username,
        string subject,
        MimeEntity body
    )
    {
        var address = Message.CreateAddress(username, email);
        var result = Message.CreateMessage(address, subject, body);
        return result;
    }
}
