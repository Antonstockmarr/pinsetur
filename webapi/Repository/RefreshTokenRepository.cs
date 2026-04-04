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
                ExpiresAt = DateTime.UtcNow.AddDays(30)
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

        public void Delete(string token)
        {
            _tableClient.DeleteEntity(partitionKey: "RefreshToken", rowKey: token);
        }

    }
}
