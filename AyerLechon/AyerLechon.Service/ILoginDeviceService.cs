namespace AyerLechon.Service
{
    public interface ILoginDeviceService
    {
        void AddOrUpdate(int accountId, string deviceId);
    }
}
