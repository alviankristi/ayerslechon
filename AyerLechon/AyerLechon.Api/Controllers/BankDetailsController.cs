using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AyerLechon.Model;
using AyerLechon.Repo.Domains;
using AyerLechon.Service.Implementations;

namespace AyerLechon.Api.Controllers
{
    public class BankDetailsController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            using (var ctx = new AyerLechonContext())
            {
                var response = new ResponseViewModel<string>()
                {
                    Status = new Status
                    {
                        Type = "Success",
                        Message = ""
                    },
                    Data = ctx.BankDetails.FirstOrDefault()?.Description ?? string.Empty
                };
                return Ok(response);
            }
        }
    }
}