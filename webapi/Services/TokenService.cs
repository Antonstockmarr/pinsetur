using stockmarrdk_api.Repository;

namespace stockmarrdk_api.Services
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
