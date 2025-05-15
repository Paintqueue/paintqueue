using Microsoft.AspNetCore.Mvc;
using PainterQueueApi.Models;

namespace PainterQueueApi.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines methods for interacting with a storage system, including operations for managing blobs and containers.
    /// </summary>
    /// <remarks>This interface provides functionality for retrieving, creating, and deleting blobs, as well
    /// as managing metadata and containers. Implementations of this interface are expected to handle the underlying
    /// storage system details.</remarks>
    public interface IStorageClient
    {
        /// <summary>
        /// Retrieves a blob from the specified container as a <see cref="FileStreamResult"/>.
        /// </summary>
        /// <remarks>Use this method to retrieve a blob for streaming purposes. Ensure that the specified
        /// container  and blob names are valid and exist in the storage system.</remarks>
        /// <param name="containerName">The name of the container where the blob is stored. Cannot be null or empty.</param>
        /// <param name="blobName">The name of the blob to retrieve. Cannot be null or empty.</param>
        /// <returns>A <see cref="FileStreamResult"/> representing the blob's content if the blob is found;  otherwise, <see
        /// langword="null"/>.</returns>
        Task<FileStreamResult?> GetBlob(string containerName, string blobName);

        /// <summary>
        /// Creates a new blob in the specified container with the provided metadata and content.
        /// </summary>
        /// <remarks>This method uploads the provided content to the specified blob storage container.
        /// Ensure that the container exists and that the caller has the necessary permissions to create blobs in the
        /// container.</remarks>
        /// <param name="containerName">The name of the container where the blob will be created. Must not be null or empty.</param>
        /// <param name="blobName">The name of the blob to create. Must not be null or empty.</param>
        /// <param name="stream">The content of the blob as a file stream. Must not be null.</param>
        /// <param name="metaData">The metadata to associate with the blob. Can be null if no metadata is required.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the blob was
        /// created successfully; otherwise, <see langword="false"/>.</returns>
        Task<bool> CreateBlob(string containerName, string blobName, IFormFile stream, BlobStorageDetails metaData);

        /// <summary>
        /// Deletes a blob from the specified container in the storage system.
        /// </summary>
        /// <remarks>This method performs a delete operation on the specified blob. If the blob does not
        /// exist, the method returns <see langword="false"/> without throwing an exception. Ensure that the caller has
        /// the necessary permissions to delete the blob.</remarks>
        /// <param name="containerName">The name of the container that contains the blob to delete. Cannot be null or empty.</param>
        /// <param name="blobName">The name of the blob to delete. Cannot be null or empty.</param>
        /// <returns><see langword="true"/> if the blob was successfully deleted; otherwise, <see langword="false"/> if the blob
        /// does not exist or could not be deleted.</returns>
        Task<bool> DeleteBlob(string containerName, string blobName);

        /// <summary>
        /// Deletes the specified container from the storage system.
        /// </summary>
        /// <remarks>This method attempts to delete the container with the specified name. If the
        /// container does not exist,  the method returns <see langword="false"/> without throwing an
        /// exception.</remarks>
        /// <param name="containerName">The name of the container to delete. This value cannot be null or empty.</param>
        /// <returns><see langword="true"/> if the container was successfully deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeleteContainer(string containerName);

        /// <summary>
        /// Retrieves metadata for all blobs within the specified container.
        /// </summary>
        /// <remarks>This method does not download the blob contents; it only retrieves metadata
        /// associated with each blob. Ensure the caller has appropriate permissions to access the specified
        /// container.</remarks>
        /// <param name="containerName">The name of the container from which to retrieve blob metadata. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of  <see
        /// cref="BlobStorageDetails"/> objects representing the metadata of the blobs in the container,  or <see
        /// langword="null"/> if the container does not exist or an error occurs.</returns>
        Task<ICollection<BlobStorageDetails>?> GetBlobMetaDataInContainer(string containerName);
    }
}
