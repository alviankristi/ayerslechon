using AyerLechon.Service.ViewModels;
using System.Net.Mail;

namespace AyerLechon.Service.Implementations
{
    public class EmailService : IEmailService
    {
        public void Send(EmailViewModel model)
        {
            var mail = new MailMessage();
            mail.To.Add(model.EmailTo);
            var client = new SmtpClient();
            mail.Subject = model.Subject;
            mail.Body = model.Body;
            mail.IsBodyHtml = true;
            client.Send(mail);
        }
    }
}
