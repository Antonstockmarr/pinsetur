using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using stockmarr_api.Models;
using stockmarrdk_api.Services;
using Azure;
using Microsoft.AspNetCore.Authorization;

namespace stockmarrdk_api.Repository
{
    public class AzureStorage : IAzureStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string? _storageContainerName;
        private readonly ILogger<AzureStorage> _logger;

        public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger, BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _storageContainerName = configuration.GetValue<string>("ContainerName");
            _logger = logger;
        }

        public async Task<List<BlobDto>> ListAsync()
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_storageContainerName);

            // Create a new list object for 
            List<BlobDto> files = new List<BlobDto>();

            await foreach (BlobItem file in container.GetBlobsAsync())
            {
                // Add each file retrieved from the storage container to the files list by creating a BlobDto object
                string uri = container.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }

            // Return all files to the requesting method
            return files;
        }

        public async Task<BlobResponseDto> UploadAsync(IFormFile blob)
        {
            // Create new upload response object that we can return to the requesting method
            BlobResponseDto response = new();

            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_storageContainerName);
            //await container.CreateAsync();
            try
            {
                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                BlobClient client = container.GetBlobClient(blob.FileName);

                // Open a stream for the file we want to upload
                await using (Stream? data = blob.OpenReadStream())
                {
                    // Upload the file async
                    await client.UploadAsync(data);
                }

                // Everything is OK and file got uploaded
                response.Status = $"File {blob.FileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            // If we get an unexpected error, we catch it here and return the error message
            catch (RequestFailedException ex)
            {
                // Log error to console and create a new response we can return to the requesting method
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            // Return the BlobUploadResponse object
            return response;
        }

        public async Task<BlobResponseDto> DeleteAsync(string blobFilename)
        {
            BlobContainerClient container = _blobServiceClient.GetBlobContainerClient(_storageContainerName);

            BlobClient file = container.GetBlobClient(blobFilename);

            try
            {
                // Delete the file
                await file.DeleteAsync();
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                // File did not exist, log to console and return new response to requesting method
                _logger.LogError($"File {blobFilename} was not found.");
                return new BlobResponseDto { Error = true, Status = $"File with name {blobFilename} not found." };
            }

            // Return a new BlobResponseDto to the requesting method
            return new BlobResponseDto { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };

        }

        public Task<BlobDto> DownloadAsync(string blobFilename)
        {
            throw new NotImplementedException();
        }
    }
}