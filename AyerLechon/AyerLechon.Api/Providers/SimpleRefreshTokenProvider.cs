using AyerLechon.Repo.Domains;
using AyerLechon.Repo.Impementations;
using AyerLechon.Service;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Threading.Tasks;

namespace AyerLechon.Api.Providers
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            IClientService _repo = new ClientService();
            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");
            var token = new RefreshToken()
            {
                TokenID = refreshTokenId,
                ClientId = clientid,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow.ToEpochTime(),
                ExpiresUtc = DateTime.UtcNow.AddDays(Convert.ToDouble(refreshTokenLifeTime)).ToEpochTime()
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc.ToDateTimeOffsetFromEpoch();
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc.ToDateTimeOffsetFromEpoch();

            token.ProtectedTicket = context.SerializeTicket();

            var result =  _repo.AddRefreshToken(token);

            if (result)
            {
                context.SetToken(refreshTokenId);
            }
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            Create(context);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = context.Token;

            IClientService _repo = new ClientService();
            var refreshToken =  _repo.FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result =  _repo.RemoveRefreshToken(hashedTokenId);
            }
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            Receive(context);
        }


    }
}