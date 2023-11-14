using stockmarrdk_api.Models;

namespace stockmarrdk_api.Repository
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();

        User? GetUserFromUserName(string UserName);

        void CreateUser(User user);

        bool UserNameExists(string userName);

        void UpdateUser(User user);
    }
}
