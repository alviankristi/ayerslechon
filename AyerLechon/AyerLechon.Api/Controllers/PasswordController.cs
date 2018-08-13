using AyerLechon.Api.Helpers;
using AyerLechon.Api.Models;
using AyerLechon.Service;
using AyerLechon.Service.Implementations;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AyerLechon.Api.Controllers
{
    //api/password
    [RoutePrefix("api/password")]
    public class PasswordController : ApiController
    {
        private IAccountService _accountService = new AccountService();

        // POST api/password/change
        [Route("change")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage Change(ChangePasswordViewModel model)
        {
            ResponseViewModel<object> response = null;
            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.ChangePassword(model, UserProvider.GetId());
                    response = new ResponseViewModel<object>
                    {
                        Status = new Status()
                        {
                            Type = "Success",
                            Message = "The Password has been changed successfully."
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

        // GET api/password/sendEmailToReset
        [Route("sendEmailToReset")]
        [HttpGet]
        public HttpResponseMessage SendEmailToReset(string email)
        {
            ResponseViewModel<object> response = null;
            if (string.IsNullOrEmpty(email))
            {
                response = new ResponseViewModel<object>()
                {
                    Status = new Status()
                    {
                        Type = "Error",
                        Message = "The email is required",
                        FieldMessages = null
                    },
                    Data = null
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, response);
            }

            try
            {
                _accountService.SendResetPasswordMail(email);
                response = new ResponseViewModel<object>()
                {
                    Status = new Status()
                    {
                        Type = "Success",
                        Message = "The Password has been reset successfully. Please check your email."
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

        // GET api/password/reset
        [Route("reset")]
        [HttpGet]
        public IHttpActionResult Reset(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("The token is required");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _accountService.ResetPassword(token);

                    return Redirect(ConfigurationManager.AppSettings["BaseUrl"] + "Message/ResetPasswordSuccessfully");
                }
                catch (ApplicationException ae)
                {
                    return BadRequest(ae.Message);
                }
            }
            return BadRequest(ModelState);
        }
    }
}
