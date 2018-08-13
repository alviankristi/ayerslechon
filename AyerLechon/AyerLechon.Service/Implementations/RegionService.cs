using AyerLechon.Model;
using AyerLechon.Repo.Domains;
using System.Collections.Generic;
using System.Linq;

namespace AyerLechon.Service.Implementations
{
    public class RegionService
    {
        public IEnumerable<RegionViewModel> GetAll()
        {
            using (var ctx = new AyerLechonContext())
            {
                return ctx.Regions.Where(a => a.ShowOnApp).Select(a => new RegionViewModel()
                {
                    Id = a.RegionID,
                    Name = a.Name,
                    DeliveryFee = a.DeliveryFee,
                    IsPickupAtStore = a.IsPickupAtStore,
                    IsAirFreight = a.IsAirFreight
                }).OrderBy(a => a.Name).ToList();
            }
        }
    }
}
