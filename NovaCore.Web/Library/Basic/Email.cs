using System.Net.Mail;

namespace NovaCore.Web
{
    public class Email
    {
        public readonly MailAddress Sender;
        public readonly MailAddress Recipient;
        public readonly string Subject;
        public readonly string Body;

        public Email(string sender, string recipient)
        {
            Sender = new MailAddress(sender);
            Recipient = new MailAddress(recipient);
        }
        
        public Email(string sender, string recipient, string subject, string body)
        {
            Subject = subject;
            Body = body;
            Sender = new MailAddress(sender);
            Recipient = new MailAddress(recipient);
        }

        public MailMessage Generate()
        {
            MailMessage mailMessage = new()
            {
                From = Sender, 
                Subject = Subject,
                IsBodyHtml = true, 
                Body = Body
            };
            mailMessage.To.Add(Recipient);
            return mailMessage;
        }

        public static void SendEmail(Email email)
        {
            new SmtpClient("localhost", 587).Send(email.Generate());
        }
    }
}