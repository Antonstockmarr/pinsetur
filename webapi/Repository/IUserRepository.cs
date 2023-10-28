using stockmarrdk_api.Models;

namespace stockmarrdk_api.Repository
{
    public interface IUserRepository
    {
        User? GetUserFromUserName(string UserName);

        void CreateUser(User user);

        bool UserNameExists(string userName);

        void UpdateUser(User user);
    }
}
