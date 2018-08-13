using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using AyerLechon.Repo.Domains;

namespace AyerLechon.Api.Controllers
{
    public class FacebookLoginController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }
            using (var context = new AyerLechonContext())
            {
                var customer = context.Customers.Include("LoginDevices").FirstOrDefault(a => a.Email == model.Email);
                if (customer == null)
                {
                    customer = new Customer()
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        VIP = false
                    };
                }
                var device = context.LoginDevices.FirstOrDefault(a => a.DeviceId == model.DeviceId);
                if (device != null)
                {
                    context.LoginDevices.Remove(device);
                }
                var newdevice = new LoginDevice()
                {
                    DeviceId = model.DeviceId,
                    CreateDate = DateTime.Now.ToEpochTime(),
                    FbAccountId = verifiedAccessToken.user_id,
                    LastLoginDate = DateTime.Now.ToEpochTime(),
                };
                customer.LoginDevices.Add(newdevice);
                context.SaveChanges();

                var accessTokenResponse = GenerateLocalAccessTokenResponse(customer);

                return Ok(accessTokenResponse);
            }


        }

        [AllowAnonymous]
        [HttpGet]
        private JObject GenerateLocalAccessTokenResponse(Customer user)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            identity.AddClaim(new Claim("username", user.Email));
            identity.AddClaim(new Claim("userid", user.CustomerID.ToString()));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                new JProperty("userName", user.Email),
                new JProperty("access_token", accessToken),
                new JProperty("token_type", "bearer"),
                new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
            );

            return tokenResponse;
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            //You can get it from here: https://developers.facebook.com/tools/accesstoken/
            //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
            //var input_token =
            //    "EAAWY9ZASvHHIBAMufYZBLk8NHRAZC3AE0FWgZBkEqyhLqAIiyF5XHB6qSX2o9JqMzDv5p3wiROzCJMVdpJtC5MrZCxZAzrpNqQQwrBgQBa36qjkL7jdp9jKl2waf2jWMcqisDyEKdpKCjd0OceTpaOG75vMYC739z8htZCyYoDKSFEG1ZAS254HXgIbcNUiASVgZD";
            var fbAppSecredId = ConfigurationManager.AppSettings["FbAppId"] + '|' + ConfigurationManager.AppSettings["FbAppSecret"];
            var verifyTokenEndPoint = string.Format("https://graph.facebook.com/v2.10/debug_token?input_token={0}&access_token={1}", accessToken, fbAppSecredId);

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken
                {
                    user_id = jObj["data"]["user_id"],
                    app_id = jObj["data"]["app_id"]
                };

                if (!string.Equals(Startup.FacebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
            }

            return parsedToken;
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }


    }



    public class RegisterExternalBindingModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string DeviceId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public string ExternalAccessToken { get; set; }

    }
    public class ParsedExternalAccessToken
    {
        public string user_id { get; set; }
        public string app_id { get; set; }
    }
}
