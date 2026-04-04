using Microsoft.IdentityModel.Tokens;
using Pinsetur.Webapi.Dto;
using Pinsetur.Webapi.Models;
using Pinsetur.Webapi.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pinsetur.Webapi.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _jwtKey = configuration.GetValue<string>("JwtKey") ?? throw new Exception("JwtKey could not be read");
            _jwtIssuer = configuration["JwtSettings:Issuer"] ?? throw new Exception("JwtSettings:Issuer could not be read");
            _jwtAudience = configuration["JwtSettings:Audience"] ?? throw new Exception("JwtSettings:Audience could not be read");
        }

        public string CreateRefreshToken(string userName)
        {
            return _refreshTokenRepository.Create(userName).Token;
        }

        public (SessionDto session, string newToken)? Refresh(string token)
        {
            var refreshToken = _refreshTokenRepository.Get(token);
            if (refreshToken is null || refreshToken.ExpiresAt < DateTime.UtcNow)
                return null;

            var user = _userRepository.GetUserFromUserName(refreshToken.UserName);
            if (user is null)
                return null;

            // Rotate: delete old token, issue new one
            _refreshTokenRepository.Delete(token);
            var newToken = _refreshTokenRepository.Create(user.UserName);

            var session = new SessionDto { User = user.ToUserDto(), Jwt = CreateJwt(user) };
            return (session, newToken.Token);
        }

        public void Delete(string token)
        {
            _refreshTokenRepository.Delete(token);
        }

        private string CreateJwt(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
