using stockmarrdk_api.Common;
using stockmarrdk_api.Dto;
using stockmarrdk_api.Models;
using stockmarrdk_api.Repository;
using System.Security.Cryptography;

namespace stockmarrdk_api.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private static readonly Random random = new Random();

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
    }

        public string CreateUser(NewUserDto newUserDto)
        {
            if (_userRepository.UserNameExists(newUserDto.UserName))
            {
                throw new AlreadyExistsException("A user with that username already exists");
            }

            var initialPassword = RandomString(6);
            var newUser = new User {
                UserName = newUserDto.UserName,
                Role = newUserDto.Role,
                Name = newUserDto.Name,
                ResetPassword = true

            };
            newUser = SetHashes(newUser, initialPassword);
            _userRepository.CreateUser(newUser);
            return initialPassword;
        }

        public UserDto? UpdatePassword(string userName, string newPassword)
        {
            // TODO: Validate user token before
            User? user = _userRepository.GetUserFromUserName(userName);
            if (user is null)
            {
                return null;
            }
            else
            {
                user = SetHashes(user, newPassword);
                user.ResetPassword = false;
                _userRepository.UpdateUser(user);
                return user.ToUserDto();
            }
        }

        private static User SetHashes(User user, string password)
        {
            using var hmac = new HMACSHA512();
            user.PasswordSalt = hmac.Key;
            user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return user;
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
