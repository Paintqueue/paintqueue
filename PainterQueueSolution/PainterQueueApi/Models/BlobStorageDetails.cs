using Newtonsoft.Json.Schema;
using NJsonSchema.Annotations;
using System.Text.Json.Serialization;

namespace PainterQueueApi.Models
{
    /// <summary>
    /// Represents the details of a blob stored in Azure Blob Storage, including metadata and status information.
    /// </summary>
    /// <remarks>This class provides properties to describe the container, file, and blob names, as well as
    /// the success status and the timestamp indicating when the blob was inserted. It's purpose is usually to 
    /// get information in files in a container.</remarks>
    public class BlobStorageDetails
    {
        [JsonSchemaType(typeof(string))]
        [JsonPropertyName("containerName")]
        public string ContainerName { get; set; } = "";

        [JsonSchemaType(typeof(string))]
        [JsonPropertyName("fileName")]
        public string FileName { get; set; } = "";

        [JsonSchemaType(typeof(string))]
        [JsonPropertyName("blobName")]
        public string BlobName { get; set; } = "";

        [JsonSchemaType(typeof(string))]
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; } = true;

        [JsonSchemaType(typeof(string))]
        [JsonPropertyName("insertedOn")]
        public DateTimeOffset InsertedOn { get; set; }
    }
}
