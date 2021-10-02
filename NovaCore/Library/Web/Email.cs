using System.Net.Mail;

namespace NovaCore.Library.Web
{
    public class Email
    {
        public MailAddress Sender;
        public MailAddress Recipient;
        public string Subject;
        public string Body;

        public Email(string sender, string recipient)
        {
            Sender = new MailAddress(sender);
            Recipient = new MailAddress(recipient);
        }

        public MailMessage Generate()
        {
            MailMessage mailMessage = new MailMessage
                {From = Sender, Subject = Subject, IsBodyHtml = true, Body = Body};
            mailMessage.To.Add(Recipient);
            return mailMessage;
        }

        public static void SendEmail(Email email)
        {
            new SmtpClient("localhost", 587).Send(email.Generate());
        }
    }
}