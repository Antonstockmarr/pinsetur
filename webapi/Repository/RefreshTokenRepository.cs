using Azure.Data.Tables;
using Pinsetur.Webapi.Models;
using Pinsetur.Webapi.TableEntities;

namespace Pinsetur.Webapi.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly TableClient _tableClient;

        public RefreshTokenRepository(IConfiguration configuration, TableServiceClient tableServiceClient)
        {
            string? tableName = configuration.GetValue<string>("RefreshTokensTableName");
            _tableClient = tableServiceClient.GetTableClient(tableName);
            _tableClient.CreateIfNotExists();
        }

        public RefreshToken Create(string userName)
        {
            var token = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserName = userName,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                Revoked = false
            };
            _tableClient.AddEntity(new RefreshTokenEntity(token));
            return token;
        }

        public RefreshToken? Get(string token)
        {
            var entity = _tableClient
                .Query<RefreshTokenEntity>(e => e.PartitionKey == "RefreshToken" && e.RowKey == token)
                .SingleOrDefault();
            return entity?.ToRefreshToken();
        }

        public void Revoke(string token)
        {
            var entity = _tableClient
                .Query<RefreshTokenEntity>(e => e.PartitionKey == "RefreshToken" && e.RowKey == token)
                .SingleOrDefault();
            if (entity is not null)
            {
                entity.Revoked = true;
                _tableClient.UpdateEntity(entity, entity.ETag, TableUpdateMode.Replace);
            }
        }
    }
}
