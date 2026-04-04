using Azure;
using Azure.Data.Tables;
using Pinsetur.Webapi.Models;

namespace Pinsetur.Webapi.TableEntities
{
    public class RefreshTokenEntity : ITableEntity
    {
        public string UserName { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool Revoked { get; set; }

        // ITableEntity Properties
        public string PartitionKey { get; set; } = "RefreshToken";
        public string RowKey { get; set; } = string.Empty; // The token (Guid)
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public RefreshTokenEntity() { }

        public RefreshTokenEntity(RefreshToken token)
        {
            RowKey = token.Token;
            UserName = token.UserName;
            ExpiresAt = token.ExpiresAt;
            Revoked = token.Revoked;
        }

        public RefreshToken ToRefreshToken()
        {
            return new RefreshToken
            {
                Token = RowKey,
                UserName = UserName,
                ExpiresAt = ExpiresAt,
                Revoked = Revoked
            };
        }
    }
}
