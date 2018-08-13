using AyerLechon.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AyerLechon.Repo.Impementations
{
    public class AuthRepo : BaseRepo<Client>, IBaseRepo<Client>, IAuthRepo
    {
        private IAccountRepo _accountRepo;

        public AuthRepo(AppContext appContext) : base(appContext)
        {
            _accountRepo = new AccountRepo(appContext);
        }

        public Client FindClient(string clientId)
        {
            var client = _dbSet.Find(Guid.Parse(clientId));

            return client;
        }

        public bool AddRefreshToken(RefreshToken token)
        {

            var existingToken = _context.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

            if (existingToken != null)
            {
                var result = RemoveRefreshToken(existingToken);
            }

            _context.RefreshTokens.Add(token);

            return _context.SaveChanges() > 0;
        }

        public bool RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = _context.RefreshTokens.Find(refreshTokenId);

            if (refreshToken != null)
            {
                _context.RefreshTokens.Remove(refreshToken);
                return _context.SaveChanges() > 0;
            }

            return false;
        }

        public bool RemoveRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
            return _context.SaveChanges() > 0;
        }

        public RefreshToken FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = _context.RefreshTokens.Find(refreshTokenId);
            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _context.RefreshTokens.ToList();
        }


    }
}
