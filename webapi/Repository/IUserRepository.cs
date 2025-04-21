using Pinsetur.Webapi.Models;

namespace Pinsetur.Webapi.Repository
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();

        User? GetUserFromUserName(string UserName);

        void CreateUser(User user);

        bool UserNameExists(string userName);

        void UpdateUser(User user);

        User? DeleteUser(string userName);
    }
}
