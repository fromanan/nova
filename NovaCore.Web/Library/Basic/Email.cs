using System.Net.Mail;

namespace NovaCore.Web;

public record Email(string Sender, string Recipient, string Subject, string Body)
{
    public void Send(string host = "localhost", int port = Hosts.DefaultPort)
    {
        MailMessage mailMessage = new()
        {
            From = new MailAddress(Sender), 
            Subject = Subject,
            IsBodyHtml = true, 
            Body = Body,
            To = { new MailAddress(Recipient) }
        };

        using SmtpClient client = new(host, port);
        
        client.Send(mailMessage);
    }
}