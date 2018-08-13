using AyerLechon.Repo.Domains;
using AyerLechon.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AyerLechon.Repo.Impementations
{
    public class ClientService : IClientService
    {
        AyerLechonContext _ctx;
        public ClientService(AyerLechonContext ctx)
        {
            ctx = ctx;
        }

        public ClientService()
        {
        }

        public Client Find(Guid clientId)
        {
            using (var ctx = _ctx ?? new AyerLechonContext())
            {
                return ctx.Clients.FirstOrDefault(a => a.ClientID == clientId);
            }

        }

        public bool AddRefreshToken(RefreshToken token)
        {
            using (var ctx = new AyerLechonContext())
            {
                var existingToken = ctx.RefreshTokens.FirstOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

                if (existingToken != null)
                {
                    ctx.RefreshTokens.Remove(existingToken);
                }

                ctx.RefreshTokens.Add(token);

                return ctx.SaveChanges() > 0;
            }
        }

        public bool RemoveRefreshToken(string refreshTokenId)
        {
            using (var ctx = _ctx ?? new AyerLechonContext())
            {
                var refreshToken = ctx.RefreshTokens.Find(refreshTokenId);

                if (refreshToken != null)
                {
                    ctx.RefreshTokens.Remove(refreshToken);
                    return ctx.SaveChanges() > 0;
                }

                return false;
            }
        }

        private void RemoveRefreshToken(RefreshToken refreshToken)
        {
            using (var ctx = _ctx ?? new AyerLechonContext())
            {
                ctx.RefreshTokens.Remove(refreshToken);
                ctx.SaveChanges();
            }
        }

        public RefreshToken FindRefreshToken(string refreshTokenId)
        {
            using (var ctx = _ctx ?? new AyerLechonContext())
            {
                var refreshToken = ctx.RefreshTokens.Find(refreshTokenId);
                return refreshToken;
            }
        }

        public IEnumerable<RefreshToken> GetAllRefreshTokens()
        {
            using (var ctx = _ctx ?? new AyerLechonContext())
            {
                return ctx.RefreshTokens.ToList();
            }
        }


    }
}
