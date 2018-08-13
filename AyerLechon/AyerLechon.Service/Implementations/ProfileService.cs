using AyerLechon.Model;
using AyerLechon.Repo.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyerLechon.Service.Implementations
{
    public class ProfileService
    {
        private AyerLechonContext _context;
        public ProfileService(AyerLechonContext ctx)
        {
            _context = ctx;
        }

        public ProfileViewModel Get(int userId)
        {
            var customer = _context.Customers.FirstOrDefault(a => a.CustomerID == userId);

            return new ProfileViewModel()
            {
                Address = customer.Address,
                FirstName = customer.FirstName,
                Id = customer.CustomerID,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                RegionId = customer.RegionID,
                Email = customer.Email,
                NewVipApplication = customer.NewVIPApplication
            };
        }

        public void Update(ProfileViewModel model)
        {
            var customer = _context.Customers.FirstOrDefault(a => a.CustomerID == model.Id);

            if (customer.Email != model.Email)
            {
                var otherAccount = _context.Customers.FirstOrDefault(a => a.Email == model.Email);
                if (otherAccount != null)
                {
                    throw new ApplicationException("The email is already exist");
                }
            }
            _context.Customers.Attach(customer);
            customer.RegionID = model.RegionId;
            customer.PhoneNumber = model.PhoneNumber;
            customer.LastName = model.LastName;
            customer.FirstName = model.FirstName;
            customer.Email = model.Email;
            customer.Address = model.Address;
            _context.SaveChanges();
        }
    }
}
