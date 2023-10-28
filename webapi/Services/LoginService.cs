using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;
using stockmarrdk_api.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace stockmarrdk_api.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly string _jwtKey;

        public LoginService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            string? jwtKey = _configuration.GetValue<string>("JwtKey");
            if (jwtKey is null)
            {
                throw new Exception("jwtKey could not be read");
            }
            else
            {
                _jwtKey = jwtKey;
            }
        }

        public SessionDto? Login(LoginDto login)
        {
            User? user = _userRepository.GetUserFromUserName(login.UserName);

            if (user is not null && ValidateLogin(user, login))
            {
                string jwt = CreateJwt(user);
                return new SessionDto { User = user.ToUserDto(), Jwt = jwt };
            }
            return null;
        }

        private static bool ValidateLogin(User user, LoginDto login)
        {
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(login.Password));
                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }

        private string CreateJwt(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
             };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                                   claims: claims,
                                   expires: DateTime.UtcNow.AddDays(1),
                                   signingCredentials: cred
   );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
