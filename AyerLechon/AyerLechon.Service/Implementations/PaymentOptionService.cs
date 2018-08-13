using AyerLechon.Model;
using AyerLechon.Repo.Domains;
using System.Collections.Generic;
using System.Linq;

namespace AyerLechon.Service.Implementations
{
    public class PaymentOptionService
    {
        private AyerLechonContext _context;
        public PaymentOptionService(AyerLechonContext context)
        {
            _context = context;
        }
        public IEnumerable<PaymentOptionViewModel> GetAll(int userId)
        {
            var user = _context.Customers.FirstOrDefault(a => a.CustomerID == userId);
            if (user.VIP)
                return _context.PaymentOptions.Select(a => new PaymentOptionViewModel()
                {
                    Id = a.PaymentOptionID,
                    Name = a.PaymentOption1,
                    Charge = a.Charge / 100 ?? 0,
                    BankDetailLine1 = a.BankDetailLine1,
                    BankDetailLine2 = a.BankDetailLine2,
                    BankDetailLine3 = a.BankDetailLine3
                }).ToList();

            return _context.PaymentOptions.Where(a => a.PaymentOptionID != PaymentOptionEnum.CreditLine).Select(a => new PaymentOptionViewModel()
            {
                Id = a.PaymentOptionID,
                Name = a.PaymentOption1,
                Charge = a.Charge / 100 ?? 0,
                BankDetailLine1 = a.BankDetailLine1,
                BankDetailLine2 = a.BankDetailLine2,
                BankDetailLine3 = a.BankDetailLine3
            }).ToList();
        }
    }
}
