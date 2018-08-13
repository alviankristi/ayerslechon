using AyerLechon.Model;
using AyerLechon.Repo.Domains;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyerLechon.Service.Implementations
{
    public class DiscountService
    {
        private readonly AyerLechonContext _context;
        public DiscountService(AyerLechonContext context)
        {
            _context = context;
        }
        public IEnumerable<DiscountViewModel> GetAll(int userId)
        {
            var path = ConfigurationManager.AppSettings["BaseUrl"] + "api/images/";
            var usedPromo = UsedPromo(userId);
            var now = DateTime.Now.ToEpochTime();
            return _context.Discounts.Where(a => a.ExpiredDate >= now).Select(a => new DiscountViewModel()
            {
                Id = a.DiscountID,
                Code = a.Code,
                Description = a.Description,
                ImageUrl = path + a.Image,
                ItemId = a.ItemID,
                Enabled = !usedPromo.Any(b => b == a.DiscountID),
                Amount = a.Amount,
                Percentage = a.Percentage
            }).ToList();
        }

        public IEnumerable<int> UsedPromo(int userId)
        {
            return _context.OrderSummaries.Where(a => a.CustomerID == userId && a.DiscountId.HasValue).Select(a => a.DiscountId.Value).ToList();
        }
    }
}
