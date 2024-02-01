using Azure.Data.Tables;
using stockmarrdk_api.Models;
using stockmarrdk_api.TableEntities;

namespace stockmarrdk_api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TableClient _userTableClient;

        public UserRepository(IConfiguration configuration, TableServiceClient tableServiceClient)
        {
            string? storageTableName = configuration.GetValue<string>("UsersTableName");
            _userTableClient = tableServiceClient.GetTableClient(storageTableName);
        }

        public List<User> GetAllUsers()
        {
            return _userTableClient.Query<UserEntity>().Select(userEntity => userEntity.ToUser()).OrderByDescending(user => user.UserName).ToList();
        }

        public User? GetUserFromUserName(string userName)
        {
            UserEntity? userEntity = _userTableClient.Query<UserEntity>(userEntity => userEntity.PartitionKey == "User" && userEntity.RowKey == userName).SingleOrDefault();
            if (userEntity is null)
                return null;
            else
            {
                return (User?)userEntity.ToUser();
            }
        }

        public void CreateUser(User user)
        {
            _userTableClient.AddEntity(new UserEntity(user));
        }

        public bool UserNameExists(string userName)
        {
            return _userTableClient.Query<UserEntity>(userEntity => userEntity.PartitionKey == "User" && userEntity.RowKey == userName).Any();
        }

        public void UpdateUser(User user)
        {
            _userTableClient.UpdateEntity(entity: new UserEntity(user), ifMatch: Azure.ETag.All, mode: TableUpdateMode.Replace);
        }

        public User? DeleteUser(string userName)
        {
            User? user = GetUserFromUserName(userName);
            _userTableClient.DeleteEntity(partitionKey: "User", rowKey: userName);
            return user;
        }
    }
}
