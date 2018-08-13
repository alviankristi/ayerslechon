using AyerLechon.Api.Models;
using AyerLechon.Repo.Domains;
using AyerLechon.Repo.Impementations;
using AyerLechon.Service.ViewModels;
using System;
using System.Configuration;
using System.Linq;
using System.Text;

namespace AyerLechon.Service.Implementations
{
    public class AccountService : IAccountService
    {
        IEmailService _emailService = new EmailService();

        private bool IsExist(string email)
        {
            using (var ctx = new AyerLechonContext())
            {
                return ctx.Customers.Any(a => a.Email == email);
            }
        }

        public void Create(Customer model)
        {
            using (var ctx = new AyerLechonContext())
            {
                if (IsExist(model.Email))
                    throw new ApplicationException("The email is already exist.");

                ctx.Customers.Add(model);
                ctx.SaveChanges();
            }
        }

        public void ChangePassword(ChangePasswordViewModel model, int userId)
        {
            using (var ctx = new AyerLechonContext())
            {
                var account = ctx.Customers.FirstOrDefault(a => a.CustomerID == userId && model.CurrentPassword == a.Password);
                if (account == null)
                {
                    throw new ApplicationException("The current password is incorrect.");
                }
                ctx.Customers.Attach(account);
                account.Password = model.NewPassword;
                account.LastChangePassword = DateTimeOffset.Now.ToEpochTime();
                ctx.SaveChanges();
            }
        }

        public void SendResetPasswordMail(string email)
        {
            using (var ctx = new AyerLechonContext())
            {
                var account = ctx.Customers.FirstOrDefault(a => a.Email == email);
                if (account == null)
                {
                    throw new ApplicationException("Email is not registered.");
                }
                ctx.Customers.Attach(account);

                account.ResetPasswordToken = Guid.NewGuid();

                var resetPasswordUrl = ConfigurationManager.AppSettings["BaseUrl"] + "api/password/reset?token=" + account.ResetPasswordToken;

                var body = new StringBuilder();
                body.AppendFormat("<p>Dear {0}, </p>", account.FirstName + " " + account.LastName);
                body.AppendFormat("<p>We received a request to change your password on <a href=\"{0}\">Ayer Lechon</a>. </p>", "http://ayerlechon.com");
                body.Append("<p>Click the link below to set a new password: </p>");
                body.AppendFormat("<h1><a href=\"{0}\">Reset Password</a></h1>", resetPasswordUrl);
                body.AppendFormat("<p>If you do not want to change your password you can ignore this email.</p>", "ResetPassword");
                body.Append("<p>Thanks, </p>");
                body.Append("<p>Ayer Lechon</p>");

                var emailModel = new EmailViewModel()
                {
                    Body = body.ToString(),
                    EmailTo = email,
                    Subject = "Password Reset"
                };

                _emailService.Send(emailModel);
                ctx.SaveChanges();
            }
        }
        public static string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void ResetPassword(string token)
        {
            using (var ctx = new AyerLechonContext())
            {
                var tkn = Guid.Parse(token);
                var account = ctx.Customers.FirstOrDefault(a => a.ResetPasswordToken == tkn);
                if (account == null)
                {
                    throw new ApplicationException("The token is expired. Please reset your password again.");
                }
                ctx.Customers.Attach(account);

                account.Password = RandomString(6);
                account.ResetPasswordToken = null;
                account.LastChangePassword = DateTimeOffset.Now.ToEpochTime();

                var body = new StringBuilder();
                body.AppendFormat("<p>Dear {0}, </p>", account.FirstName + " " + account.LastName);
                body.Append("<p>The password has been reset.</p>");
                body.Append("<p>You can now log in with the following credentials:</p>");
                body.AppendFormat("<p>Username: {0}</p>", account.Email);
                body.AppendFormat("<p>New Password: {0}</p>", account.Password);
                body.Append("<br/><br/><p>Thanks, </p>");
                body.Append("<p>Ayer Lechon</p>");

                var emailModel = new EmailViewModel()
                {
                    Body = body.ToString(),
                    EmailTo = account.Email,
                    Subject = "Your new password"
                };

                _emailService.Send(emailModel);

                ctx.SaveChanges();
            }
        }

        public Customer Login(string username, string password, string deviceId)
        {
            using (var ctx = new AyerLechonContext())
            {
                ILoginDeviceService loginDeviceService = new LoginDeviceService(ctx);
                var account = ctx.Customers.FirstOrDefault(a => a.Email == username && a.Password == password);
                if (account != null)
                {
                    loginDeviceService.AddOrUpdate(account.CustomerID, deviceId);
                }
                return account;
            }
        }


    }
}
