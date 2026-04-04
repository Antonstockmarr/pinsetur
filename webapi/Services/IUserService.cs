using Pinsetur.Webapi.Dto;

namespace Pinsetur.Webapi.Services
{
    public interface IUserService
    {
        string? CreateUser(NewUserDto newUser);
        List<UserDto> ListUsers();
        string? ResetPassword(string userName);
        UserDto? UpdatePassword(string userName, string newPassword);
        UserDto? UpdateUser(UserDto user);
        UserDto? DeleteUser(UserDto user);
    }
}
