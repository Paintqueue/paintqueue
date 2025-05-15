using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using PainterQueueApi.Infrastructure.Interfaces;
using PainterQueueApi.Models;
using System.Runtime;

namespace PainterQueueApi.Infrastructure.AzureServices
{
    /// <summary>
    /// Provides methods for interacting with Azure Blob Storage, including operations for managing blobs and containers
    /// such as retrieving, creating, deleting, and retrieving metadata.
    /// </summary>
    /// <remarks>This class is designed to simplify common Azure Blob Storage operations by providing
    /// high-level abstractions for blob and container management. It includes methods for retrieving blobs as file
    /// streams, uploading files with metadata, deleting blobs and containers, and retrieving metadata for blobs in a
    /// container. Errors encountered during operations are logged using the provided <see
    /// cref="ILogger{TCategoryName}"/> instance. <para> The class requires an instance of <see
    /// cref="AzureStorageSettings"/> to configure the connection to Azure Blob Storage and an <see
    /// cref="ILogger{TCategoryName}"/> for logging purposes. </para></remarks>
    public class AzureStorageClient : IStorageClient
    {
        private readonly ILogger<AzureStorageClient> _logger;
        private readonly AzureStorageSettings _settings;

        public AzureStorageClient(ILogger<AzureStorageClient> logger, AzureStorageSettings azureStorageSettings)
        {
            _logger = logger;
            _settings = azureStorageSettings;
        }

        /// <summary>
        /// Retrieves a blob from the specified container and returns it as a downloadable file stream.
        /// </summary>
        /// <remarks>The returned <see cref="FileStreamResult"/> includes the blob's content and content
        /// type. If the blob's metadata contains a "filename" entry, it will be used as the file download name;
        /// otherwise, the blob name will be used.</remarks>
        /// <param name="containerName">The name of the container where the blob is stored. Cannot be null or empty.</param>
        /// <param name="blobName">The name of the blob to retrieve. Cannot be null or empty.</param>
        /// <returns>A <see cref="FileStreamResult"/> containing the blob's content and metadata, or <see langword="null"/> if
        /// the blob does not exist or an error occurs during retrieval.</returns>
        public async Task<FileStreamResult?> GetBlob(string containerName, string blobName)
        {
            try
            {
                var blobClient = GetBlobClient(containerName, blobName);

                if (!await BlobExists(blobClient))
                {
                    return null;
                }

                var blobDownloadInfo = await blobClient.DownloadAsync();
                var properties = await blobClient.GetPropertiesAsync();
                properties.Value.Metadata.TryGetValue("filename", out var fileName);
                fileName ??= blobName;

                return new FileStreamResult(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType ?? "application/octet-stream")
                {
                    FileDownloadName = fileName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blob {BlobName} in container {ContainerName}", blobName, containerName);

                return null;
            }
        }

        /// <summary>
        /// Creates a new blob in the specified container with the provided content and metadata.
        /// </summary>
        /// <remarks>If an error occurs during the creation of the blob, the method logs the error and
        /// returns <see langword="false"/>.</remarks>
        /// <param name="containerName">The name of the container where the blob will be created. Must not be null or empty.</param>
        /// <param name="blobName">The name of the blob to create. Must not be null or empty.</param>
        /// <param name="stream">The file content to upload as the blob. Must not be null.</param>
        /// <param name="metaData">The metadata to associate with the blob. Must not be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the blob was
        /// created successfully; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> CreateBlob(string containerName, string blobName, IFormFile stream, BlobStorageDetails metaData)
        {
            try
            {
                var blobClient = await CreateBlobClient(containerName, blobName);

                await UploadFile(blobClient, stream);

                await SetMetaData(blobClient, metaData);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blob {BlobName} in container {ContainerName}", blobName, containerName);

                return false;
            }
        }

        /// <summary>
        /// Deletes a blob from the specified container if it exists.
        /// </summary>
        /// <remarks>This method attempts to delete the specified blob from the given container.  If the
        /// blob does not exist, the method returns <see langword="false"/> without throwing an exception. Any errors
        /// encountered during the deletion process are logged, and the method will return <see
        /// langword="false"/>.</remarks>
        /// <param name="containerName">The name of the container that contains the blob to delete. Cannot be null or empty.</param>
        /// <param name="blobName">The name of the blob to delete. Cannot be null or empty.</param>
        /// <returns><see langword="true"/> if the blob was successfully deleted;  <see langword="false"/> if the blob does not
        /// exist or an error occurred during the deletion process.</returns>
        public async Task<bool> DeleteBlob(string containerName, string blobName)
        {
            try
            {
                var blobClient = GetBlobClient(containerName, blobName);

                if (!await blobClient.ExistsAsync())
                {
                    return false;
                }

                await blobClient.DeleteIfExistsAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blob {BlobName} in container {ContainerName}", blobName, containerName);

                return false;
            }

        }

        /// <summary>
        /// Retrieves metadata for all blobs in the specified container.
        /// </summary>
        /// <remarks>This method retrieves metadata such as the blob name, file name, success status, and
        /// insertion date for each blob in the specified container.  If the container does not exist, an empty
        /// collection is returned. If an error occurs, the method logs the error and returns <see
        /// langword="null"/>.</remarks>
        /// <param name="containerName">The name of the container from which to retrieve blob metadata. Cannot be null or empty.</param>
        /// <returns>A collection of <see cref="BlobStorageDetails"/> objects containing metadata for each blob in the container.
        /// Returns an empty collection if the container does not exist or contains no blobs.  Returns <see
        /// langword="null"/> if an error occurs during the operation.</returns>
        public async Task<ICollection<BlobStorageDetails>?> GetBlobMetaDataInContainer(string containerName)
        {
            try
            {
                var blobClient = GetContainerClient(containerName);
                var blobDetails = new List<BlobStorageDetails>();

                if (!await blobClient.ExistsAsync())
                {
                    return blobDetails;
                }

                await foreach (var blobItem in blobClient.GetBlobsAsync())
                {
                    var blob = blobClient.GetBlobClient(blobItem.Name);
                    var props = await blob.GetPropertiesAsync();
                    var metadata = props.Value.Metadata;
                    metadata.TryGetValue("filename", out var fileName);
                    fileName ??= blobItem.Name;

                    metadata.TryGetValue("isSuccess", out var isSuccessString);
                    isSuccessString ??= "true";
                    bool.TryParse(isSuccessString, out var isSuccess);

                    metadata.TryGetValue("insertedOn", out var insertedOnString);
                    DateTimeOffset.TryParse(insertedOnString, out var insertedOn);

                    blobDetails.Add(new BlobStorageDetails
                    {
                        ContainerName = containerName,
                        FileName = fileName,
                        BlobName = blobItem.Name,
                        IsSuccess = isSuccess,
                        InsertedOn = insertedOn
                    });
                }

                return blobDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blob metadata in container {ContainerName}", containerName);

                return null;
            }

        }

        /// <summary>
        /// Deletes the specified container if it exists.
        /// </summary>
        /// <remarks>This method checks for the existence of the container before attempting to delete it.
        /// If the container does not exist, the method returns <see langword="true"/> without performing any
        /// deletion.</remarks>
        /// <param name="containerName">The name of the container to delete. Cannot be null or empty.</param>
        /// <returns><see langword="true"/> if the container was successfully deleted or does not exist;  otherwise, <see
        /// langword="false"/> if an error occurred during the deletion process.</returns>
        public async Task<bool> DeleteContainer(string containerName)
        {
            try
            {
                var containerClient = GetContainerClient(containerName);

                if (!await containerClient.ExistsAsync())
                {
                    return true;
                }

                await containerClient.DeleteIfExistsAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting container {ContainerName}", containerName);

                return false;
            }
        }

        private async Task<BlobClient> CreateBlobClient(string containerName, string blobName)
        {
            var containerClient = await CreateContainer(containerName);

            return containerClient.GetBlobClient(blobName);
        }

        private async Task<BlobContainerClient> CreateContainer(string containerName)
        {
            var containerClient = GetContainerClient(containerName);

            if (!await containerClient.ExistsAsync())
            {
                await containerClient.CreateIfNotExistsAsync();
                await containerClient.SetAccessPolicyAsync(PublicAccessType.None);   // Set the access policy to private
            }

            return containerClient;
        }

        private BlobContainerClient GetContainerClient(string containerName)
        {
            return new BlobContainerClient(_settings.ConnectionString, containerName);
        }

        private BlobClient GetBlobClient(string containerName, string blobName)
        {
            var containerClient = GetContainerClient(containerName);

            return containerClient.GetBlobClient(blobName);
        }

        private async static Task<bool> BlobExists(BlobClient? blobClient)
        {
            if (blobClient == null || await blobClient.ExistsAsync())
            {
                return true;
            }

            return false;
        }

        private async Task UploadFile(BlobClient blobClient, IFormFile file)
        {
            using Stream fileStream = file.OpenReadStream();

            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders() { ContentType = file.ContentType });
        }

        private async Task SetMetaData(BlobClient blobClient, BlobStorageDetails metaData)
        {
            var data = new Dictionary<string, string>
            {
                { "noteid", metaData.ContainerName },
                { "filename", metaData.FileName },
                { "isSuccess", "true" }
            };

            await blobClient.SetMetadataAsync(data);
        }
    }
}
