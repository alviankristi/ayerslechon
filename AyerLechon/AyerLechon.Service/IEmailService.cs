using AyerLechon.Service.ViewModels;

namespace AyerLechon.Service
{
    public interface IEmailService
    {
        void Send(EmailViewModel model);
    }
}
