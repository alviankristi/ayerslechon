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
    public class OrderSummaryService
    {
        private AyerLechonContext _context;

        public OrderSummaryService(AyerLechonContext context)
        {
            _context = context;
        }
        public IEnumerable<OrderSummary> GetByCustomer(int id)
        {
            return _context.OrderSummaries.Where(a => a.CustomerID == id).ToList();
        }
    }
}
