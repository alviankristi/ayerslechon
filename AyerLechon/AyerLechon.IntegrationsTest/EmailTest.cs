using AyerLechon.Service.Implementations;
using AyerLechon.Service.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace AyerLechon.IntegrationsTest
{
    [TestClass]
    public class EmailTest
    {
        [TestMethod]
        public void TestEmail()
        {
            var emailService = new EmailService();
            var body = new StringBuilder();
            body.Append("<p>Email Sent!!!</p>");

            var emailModel = new EmailViewModel()
            {
                Body = body.ToString(),
                EmailTo = "alviankristi@gmail.com",
                Subject = "Test email send successfully!!!"
            };

            emailService.Send(emailModel);
        }

        [TestMethod]
        public void TestResetPassword()
        {
            var emailService = new EmailService();
            var accountService = new AccountService();
            accountService.SendResetPasswordMail("alviankristi@gmail.com");
        }
    }
}
