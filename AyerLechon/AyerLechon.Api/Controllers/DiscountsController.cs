using AyerLechon.Repo.Domains;
using AyerLechon.Service.Implementations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AyerLechon.Api.Helpers;
using AyerLechon.Model;

namespace AyerLechon.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Discounts")]
    public class DiscountsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var userId = UserProvider.GetId();

            using (var ctx = new AyerLechonContext())
            {
                var discountService = new DiscountService(ctx);
                var response = new ResponseViewModel<IEnumerable<DiscountViewModel>>()
                {
                    Status = new Status()
                    {
                        Type = "Success",
                        Message = ""
                    },
                    Data = discountService.GetAll(userId)
                };
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("Redeem")]
        public HttpResponseMessage Redeem(string voucherCode)
        {
            var userId = UserProvider.GetId();
            using (var ctx = new AyerLechonContext())
            {
                var isRedeemed = ctx.OrderSummaries.Include("Discount").Any(a =>
                     a.CustomerID == userId && a.Discount.Code.Trim() == voucherCode.Trim() && (!a.PaymentStatusId.HasValue));
                var path = ConfigurationManager.AppSettings["BaseUrl"] + "api/images/";

                if (isRedeemed)
                {
                    var discount = ctx.Discounts.FirstOrDefault(a => a.Code == voucherCode);
                    var response = new ResponseViewModel<DiscountViewModel>()
                    {
                        Status = new Status()
                        {
                            Type = "Success",
                            Message = ""
                        },
                        Data = new DiscountViewModel()
                        {
                            Code = discount.Code,
                            Description = discount.Description,
                            Id = discount.DiscountID,
                            ImageUrl = path + discount.Image
                        }
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                };

                var notRedeem = new ResponseViewModel<object>()
                {
                    Status = new Status()
                    {
                        Type = "Error",
                        Message = "The Voucher Code has been used"
                    },
                    Data = null
                };
                return Request.CreateResponse(HttpStatusCode.Forbidden, notRedeem);
            }
        }
    }
}
