using AyerLechon.Api.Helpers;
using AyerLechon.Model;
using AyerLechon.Repo.Domains;
using AyerLechon.Service.Implementations;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AyerLechon.Api.Controllers
{
    [RoutePrefix("api/Profiles")]
    [Authorize]
    public class ProfilesController : ApiController
    {
        [HttpPut]
        public HttpResponseMessage Put(ProfileViewModel model)
        {
            ResponseViewModel<object> response = null;

            if (!ModelState.IsValid)
            {
                response = new ResponseViewModel<object>()
                {
                    Status = new Status()
                    {
                        Type = "Error",
                        Message = "",
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
                    var service = new ProfileService(ctx);
                    service.Update(model);

                    response = new ResponseViewModel<object>()
                    {
                        Status = new Status()
                        {
                            Type = "Success",
                            Message = "The profile has been updated"
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
                            Message = ae.Message,
                        },
                        Data = null
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }
            }
        }

        [HttpPost]
        [Authorize]
        [Route("NewVIPApplication")]
        public HttpResponseMessage NewVIPApplication()
        {
            using (var ctx = new AyerLechonContext())
            {
                ResponseViewModel<object> response;

                try
                {
                    var userID = UserProvider.GetId();
                    var customer = ctx.Customers.FirstOrDefault(a => a.CustomerID == userID);
                    if (customer == null)
                    {
                        response = new ResponseViewModel<object>()
                        {
                            Status = new Status()
                            {
                                Type = "Error",
                                Message = "The customer is not found",
                            },
                            Data = null
                        };
                        return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                    }
                    ctx.Customers.Attach(customer);
                    customer.NewVIPApplication = true;
                    ctx.SaveChanges();
                    response = new ResponseViewModel<object>()
                    {
                        Status = new Status()
                        {
                            Type = "Success",
                            Message = "The customer has requested become vip member."
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
                            Message = ae.Message,
                        },
                        Data = null
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, response);
                }
            }
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var userID = UserProvider.GetId();
            using (var ctx = new AyerLechonContext())
            {
                var service = new ProfileService(ctx);
                var response = new ResponseViewModel<ProfileViewModel>()
                {
                    Status = new Status()
                    {
                        Type = "Success",
                        Message = ""
                    },
                    Data = service.Get(userID)
                };
                return Ok(response);
            }
        }
    }
}
