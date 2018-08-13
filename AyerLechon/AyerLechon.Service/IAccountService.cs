using AyerLechon.Api.Models;
using AyerLechon.Repo.Domains;

namespace AyerLechon.Service
{
    public interface IAccountService
    {
        void Create(Customer model);
        Customer Login(string username, string password, string deviceId);
        void ChangePassword(ChangePasswordViewModel model, int userId);
        void SendResetPasswordMail(string email);
        void ResetPassword(string token);
    }
}
