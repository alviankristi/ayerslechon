using AyerLechon.Api.Helpers;
using AyerLechon.Api.Models;
using AyerLechon.Repo.Domains;
using AyerLechon.Service;
using AyerLechon.Service.Implementations;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AyerLechon.Api.Controllers
{
    public class RegisterController : ApiController
    {
        IAccountService _accountService = new AccountService();

        // POST api/register
        public HttpResponseMessage Post(RegisterViewModel model)
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

            try
            {
                var account = new Customer
                {
                    Address = model.Address,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    RegisteredDate = DateTime.Now.ToEpochTime(),
                    VIP = false,
                    RegionID = model.IDRegion
                };
                _accountService.Create(account);
                response = new ResponseViewModel<object>()
                {
                    Status = new Status()
                    {
                        Type = "Success",
                        Message = "Congratuliations! Your account has been created successfully."
                    },
                    Data = null
                };
                return Request.CreateResponse(HttpStatusCode.OK, response);

            }
            catch (ApplicationException ap)
            {
                response = new ResponseViewModel<object>()
                {
                    Status = new Status()
                    {
                        Type = "Error",
                        Message = ap.Message
                    },
                    Data = null
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, response);
            }

        }
    }
}
