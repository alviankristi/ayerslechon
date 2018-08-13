using AyerLechon.Model;
using AyerLechon.Repo.Domains;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyerLechon.Service.Implementations
{
    public class OrderItemService
    {
        private AyerLechonContext _context;

        public OrderItemService(AyerLechonContext context)
        {
            _context = context;
        }
        public IEnumerable<ProductViewModel> GetAll()
        {
            var path = ConfigurationManager.AppSettings["BaseUrl"] + "api/images/";
            return _context.Items.Where(a => a.CategoryID == 1 && a.ReadyStock > 0)
                .Select(a => new ProductViewModel()
                {
                    Id = a.ItemID,
                    ImageUrl = a.Image.HasValue ? path + a.Image : "",
                    Name = a.Description,
                    Price = (double)a.Price,
                    ReadyStock = a.ReadyStock,
                    AirFreight = a.AirFreight
                }).ToList();
        }

        public void DecreaseStock(OrderDetail orderDetail)
        {
            var orderItem = _context.Items.FirstOrDefault(a => a.ItemID == orderDetail.ItemID);
            _context.Items.Attach(orderItem);
            orderItem.ReadyStock -= orderDetail.Quantity;
            if (orderItem.ReadyStock < 0)
            {
                throw new ApplicationException(orderItem.Description + "is out of stock");
            }
        }
    }
}
