using stockmarrdk_api.Common;

namespace stockmarrdk_api.Dto
{
    public class NewUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;

    }
}
