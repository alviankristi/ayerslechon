using System.Collections.Generic;
using AyerLechon.Repo.Domains;
using AyerLechon.Service.Implementations;
using System.Web.Http;
using AyerLechon.Model;

namespace AyerLechon.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/OrderItems")]
    public class OrderItemsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            using(var ctx = new AyerLechonContext())
            {
                var orderItemService = new OrderItemService(ctx);
                var response = new ResponseViewModel<IEnumerable<ProductViewModel>>()
                {
                    Status = new Status()
                    {
                        Type="Success",
                        Message= ""
                    },
                    Data = orderItemService.GetAll()
                };
                return Ok(response);
            }
        }
    }
}
