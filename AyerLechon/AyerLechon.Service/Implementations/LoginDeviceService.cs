using AyerLechon.Repo.Domains;
using AyerLechon.Service;
using System;
using System.Linq;

namespace AyerLechon.Repo.Impementations
{
    public class LoginDeviceService : ILoginDeviceService
    {
        AyerLechonContext _ctx;
        public LoginDeviceService(AyerLechonContext ctx)
        {
            ctx = ctx;
        }

        public void AddOrUpdate(int accountId, string deviceId)
        {
            using (var ctx = _ctx ?? new AyerLechonContext())
            {
                var loginDevice = ctx.LoginDevices.FirstOrDefault(a => a.DeviceId == deviceId && a.CustomerID == accountId);
                var account = ctx.Customers.FirstOrDefault(a => a.CustomerID == accountId);
                ctx.Customers.Attach(account);
                account.LastLogin = DateTimeOffset.Now.ToEpochTime();

                if (loginDevice != null)
                {
                    ctx.LoginDevices.Attach(loginDevice);
                    loginDevice.LastLoginDate = DateTimeOffset.Now.ToEpochTime();
                }
                else
                {
                    loginDevice = new LoginDevice()
                    {
                        CustomerID = accountId,
                        CreateDate = DateTimeOffset.Now.ToEpochTime(),
                        DeviceId = deviceId,
                        LastLoginDate = DateTimeOffset.Now.ToEpochTime()
                    };
                    ctx.LoginDevices.Add(loginDevice);
                }
            }

        }


    }
}
