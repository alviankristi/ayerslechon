using AyerLechon.Api.Helpers;
using AyerLechon.Model;
using AyerLechon.Repo.Domains;
using AyerLechon.Service.Implementations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AyerLechon.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Payments")]
    public class PaymentsController : ApiController
    {
        public HttpResponseMessage Post(OrderSummaryViewModel model)
        {
            ResponseViewModel<object> response;

            if (!ModelState.IsValid)
            {
                response = new ResponseViewModel<object>()
                {
                    Status = new Status()
                    {
                        Type = "Error",
                        FieldMessages = ModelState.ToErrorResponse()
                    },
                    Data = null
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, response);
            }


            using (var ctx = new AyerLechonContext())
            {
                try
                {
                    var service = new PaymentService(ctx);
                    service.Create(model);
                    response = new ResponseViewModel<object>()
                    {
                        Status = new Status()
                        {
                            Type = "Success",
                            Message = "The order has been created successfully."
                        },
                        Data = null
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, response);

                }
                catch (ApplicationException ae)
                {
                    response = new ResponseViewModel<object>()
                    {
                        Status = new Status()
                        {
                            Type = "Error",
                            Message = ae.Message
                        },
                        Data = null
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }
            }
        }

        [HttpGet]
        [Route("Pending")]
        public HttpResponseMessage Pending()
        {

            using (var ctx = new AyerLechonContext())
            {
                try
                {
                    var service = new PaymentService(ctx);
                    var userId = UserProvider.GetId();

                    var response = new ResponseViewModel<IEnumerable<OrderSummaryViewModel>>()
                    {
                        Status = new Status()
                        {
                            Type = "Success",
                            Message = ""
                        },
                        Data = service.GetPendings(userId)
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, response);

                }
                catch (ApplicationException e)
                {
                    var response = new ResponseViewModel<object>()
                    {
                        Status = new Status()
                        {
                            Type = "Error",
                            Message = e.Message
                        },
                        Data = null
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }
            }
        }

        [HttpPut]
        [Route("SetComplete")]
        public HttpResponseMessage SetComplete(int orderSummaryId)
        {

            using (var ctx = new AyerLechonContext())
            {
                try
                {
                    var service = new PaymentService(ctx);
                    service.SetPaid(orderSummaryId);
                    var response = new ResponseViewModel<object>()
                    {
                        Status = new Status()
                        {
                            Type = "Success",
                            Message = "The payment has been compeleted."
                        },
                        Data = null
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                catch (ApplicationException e)
                {
                    var response = new ResponseViewModel<object>()
                    {
                        Status = new Status()
                        {
                            Type = "Error",
                            Message = e.Message
                        },
                        Data = null
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }
            }
        }
    }
}
