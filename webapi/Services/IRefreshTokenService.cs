using Pinsetur.Webapi.Dto;

namespace Pinsetur.Webapi.Services
{
    public interface IRefreshTokenService
    {
        string CreateRefreshToken(string userName);
        (SessionDto session, string newToken)? Refresh(string token);
        void Delete(string token);
    }
}
