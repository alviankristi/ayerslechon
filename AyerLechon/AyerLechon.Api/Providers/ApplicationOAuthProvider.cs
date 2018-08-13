using AyerLechon.Repo.Domains;
using AyerLechon.Repo.Impementations;
using AyerLechon.Service;
using AyerLechon.Service.Implementations;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;

namespace AyerLechon.Api.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public object ApplicationTypes { get; private set; }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            using (var ctx = new AyerLechonContext())
            {
                IAccountService accountService = new AccountService();
                var deviceId = context.OwinContext.Get<string>("deviceId");

                var user = accountService.Login(context.UserName, context.Password, deviceId);

                if (user == null)
                {
                    context.SetError("The user name or password is incorrect.");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim("username", context.UserName));
                identity.AddClaim(new Claim("userid", user.CustomerID.ToString()));

                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    {
                        "userName", context.UserName
                    },
                    {
                        "deviceId", deviceId
                    }
                });
                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);

            }

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }


            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
                return Task.FromResult<object>(null);
            }

            using (var ctx = new AyerLechonContext())
            {
                IClientService _repo = new ClientService(ctx);
                client = _repo.Find(Guid.Parse(context.ClientId.Trim()));

                if (client == null)
                {
                    context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                    return Task.FromResult<object>(null);
                }

                //if (client.ApplicationType == 0)
                //{
                //    if (string.IsNullOrWhiteSpace(clientSecret))
                //    {
                //        context.SetError("invalid_clientId", "Client secret should be sent.");
                //        return Task.FromResult<object>(null);
                //    }
                //    else
                //    {
                //        if (client.Secret != clientSecret)
                //        {
                //            context.SetError("invalid_clientId", "Client secret is invalid.");
                //            return Task.FromResult<object>(null);
                //        }
                //    }
                //}

                if (!client.Active)
                {
                    context.SetError("invalid_clientId", "Client is inactive.");
                    return Task.FromResult<object>(null);
                }
                var deviceId = context.Parameters.Where(f => f.Key == "deviceid").Select(f => f.Value).SingleOrDefault()[0];
                context.OwinContext.Set("deviceId", deviceId);


                context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
                context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

                context.Validated();
                return Task.FromResult<object>(null);

            }

        }
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            //newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }


        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string>
            data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }

        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.OwinContext.Request.Method == "OPTIONS" && context.IsTokenEndpoint)
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "POST" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "accept", "authorization", "content-type" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                context.OwinContext.Response.StatusCode = 200;
                context.RequestCompleted();

                return Task.FromResult<object>(null);
            }

            return base.MatchEndpoint(context);
        }
    }
    public class FacebookAuthProvider : FacebookAuthenticationProvider
    {
        public override Task Authenticated(FacebookAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }
    }
}