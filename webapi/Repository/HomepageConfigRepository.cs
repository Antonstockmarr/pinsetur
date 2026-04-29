using Azure.Storage.Blobs;
using Pinsetur.Webapi.Models;
using System.Text.Json;

namespace Pinsetur.Webapi.Repository
{
    public class HomepageConfigRepository : IHomepageConfigRepository
    {
        private readonly BlobContainerClient _containerClient;

        public HomepageConfigRepository(IConfiguration configuration, BlobServiceClient blobServiceClient)
        {
            string containerName = configuration.GetValue<string>("ConfigContainerName") ?? "config";
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            _containerClient.CreateIfNotExists();
        }

        public HomepageConfig Get()
        {
            var blob = _containerClient.GetBlobClient("homepage.json");
            if (!blob.Exists()) return new HomepageConfig();

            var content = blob.DownloadContent();
            return JsonSerializer.Deserialize<HomepageConfig>(content.Value.Content) ?? new HomepageConfig();
        }

        public void Save(HomepageConfig config)
        {
            var blob = _containerClient.GetBlobClient("homepage.json");
            blob.Upload(BinaryData.FromObjectAsJson(config), overwrite: true);
        }
    }
}
