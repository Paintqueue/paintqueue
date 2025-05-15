using Newtonsoft.Json;
using NJsonSchema.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PainterQueueApi.Models
{
    /// <summary>
    /// Represents a data transfer object (DTO) for user.
    /// </summary>
    public class UserDto
    {
        [JsonSchemaType(typeof(string))]
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonSchemaType(typeof(string))]
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonSchemaType(typeof(string))]
        [JsonProperty("email")]
        public string? Email { get; set; }
    }
}
