using AyerLechon.Repo.Domains;
using System;
using System.Collections.Generic;

namespace AyerLechon.Service
{
    public interface IClientService
    {
        Client Find(Guid clientId);
        bool AddRefreshToken(RefreshToken token);
        bool RemoveRefreshToken(string refreshTokenId);
        RefreshToken FindRefreshToken(string refreshTokenId);
        IEnumerable<RefreshToken> GetAllRefreshTokens();
    }
}
