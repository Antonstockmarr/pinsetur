using Pinsetur.Webapi.Models;

namespace Pinsetur.Webapi.Repository
{
    public interface IRefreshTokenRepository
    {
        RefreshToken Create(string userName);
        RefreshToken? Get(string token);
        void Revoke(string token);
    }
}
