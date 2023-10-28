using stockmarrdk_api.Common;
using stockmarrdk_api.Dto;

namespace stockmarrdk_api.Models
{
    public class User
    {
        public string UserName { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public string Name { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public bool ResetPassword { get; set; } = true;

        public UserDto ToUserDto()
        {
            return new UserDto
            {
                UserName = UserName,
                Name = Name,
                Role = Role,
                ResetPassword = ResetPassword
            };
        }
    }
}
