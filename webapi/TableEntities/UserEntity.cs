using Azure;
using Azure.Data.Tables;
using stockmarrdk_api.Models;
using stockmarrdk_api.Common;

namespace stockmarrdk_api.TableEntities
{
    public class UserEntity : ITableEntity
    {
        public string UserName { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool ResetPassword { get; set; } = false;

        // ITableEntity Properties
        public virtual string PartitionKey { get; set; } = "User";
        public virtual string RowKey { get => UserName; set => UserName = value; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public UserEntity() { }

        public UserEntity(User user)
        {
            UserName = user.UserName;
            PasswordHash = user.PasswordHash;
            PasswordSalt = user.PasswordSalt;
            Name = user.Name;
            Role = user.Role.ToString();
            ResetPassword = user.ResetPassword;
            Name = user.Name;
            RowKey = UserName;
        }

        public User ToUser()
        {
            Enum.TryParse<UserRole>(Role, out var role);
            return new User { UserName = UserName, Name = Name, PasswordHash = PasswordHash, PasswordSalt = PasswordSalt, ResetPassword = ResetPassword, Role = role };
        }
    }
}
