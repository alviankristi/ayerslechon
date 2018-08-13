using System.Collections.Generic;
using AyerLechon.Api.Helpers;
using AyerLechon.Repo.Domains;
using AyerLechon.Service.Implementations;
using System.Web.Http;
using AyerLechon.Model;

namespace AyerLechon.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/PaymentOptions")]
    public class PaymentOptionsController : ApiController
    {
        // GET: PaymentOptions
        [HttpGet]
        public IHttpActionResult Get()
        {
            using (var ctx = new AyerLechonContext())
            {
                var service = new PaymentOptionService(ctx);
                var response = new ResponseViewModel<IEnumerable<PaymentOptionViewModel>>()
                {
                    Status = new Status()
                    {
                        Type = "Success",
                        Message = ""
                    },
                    Data = service.GetAll(UserProvider.GetId())
                };
                return Ok(response);
            }
        }
    }
}