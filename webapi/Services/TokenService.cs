using Pinsetur.Webapi.Repository;

namespace Pinsetur.Webapi.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;

        public TokenService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public string GetToken()
        {
            return _tokenRepository.GetToken();
        }
    }
}
