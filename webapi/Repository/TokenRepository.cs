using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace stockmarrdk_api.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly StorageSharedKeyCredential credential;

        public TokenRepository(IConfiguration configuration, BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            string accountName = _blobServiceClient.AccountName;
            string? accountKey = configuration.GetValue<string>("AccountKey");
            if (accountKey is null)
            {
                throw new Exception("AccountKey could not be read");
            }
            credential = new StorageSharedKeyCredential(accountName, accountKey);

            if (!blobServiceClient.CanGenerateAccountSasUri)
            {
                throw new Exception("Blob service client cannot create SAS tokens");
            }
        }


        public string GetToken()
        {
            AccountSasBuilder sasBuilder = new AccountSasBuilder()
            {
                Services = AccountSasServices.Blobs,
                ResourceTypes = AccountSasResourceTypes.Object,
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(1),
                Protocol = SasProtocol.Https
            };

            sasBuilder.SetPermissions(AccountSasPermissions.Read);

            return sasBuilder.ToSasQueryParameters(credential).ToString();
        }
    }
}
