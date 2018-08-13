using System.Collections.Generic;
using AyerLechon.Service.Implementations;
using System.Web.Http;
using AyerLechon.Model;

namespace AyerLechon.Api.Controllers
{
    public class RegionsController : ApiController
    {
        // POST api/regions
        public IHttpActionResult Get()
        {
            var regionService = new RegionService();
            var response = new ResponseViewModel<IEnumerable<RegionViewModel>>()
            {
                Status = new Status()
                {
                    Type="Success",
                    Message= ""
                },
                Data = regionService.GetAll()
            };
            return Ok(response);
        }
    }
}