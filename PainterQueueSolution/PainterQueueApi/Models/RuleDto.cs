using Newtonsoft.Json;
using NJsonSchema.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace PainterQueueApi.Models
{
    /// <summary>
    /// Represents a data transfer object (DTO) for a rule, containing metadata and descriptive information.
    /// </summary>
    /// <remarks>The <see cref="RuleDto"/> class is designed to store information about a specific Warhammer rule, 
    /// including its unique identifier, name, descriptions, and timestamps for creation and updates. Rules are the
    /// fundamental entity of other entities</remarks>
    public class RuleDto
    {
        [JsonSchemaType(typeof(string))]
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonSchemaType(typeof(string))]
        [StringLength(60, MinimumLength = 1)]
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonSchemaType(typeof(string))]
        [StringLength(1024, MinimumLength = 1)]
        [JsonProperty("internalDescription")]
        public string InternalDescription { get; set; } = string.Empty;

        [JsonSchemaType(typeof(string))]
        [StringLength(8192, MinimumLength = 1)]
        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonSchemaType(typeof(string))]
        [Range(0, 999)]
        [JsonProperty("Page")]
        public int Page { get; set; } = 0;

        [JsonSchemaType(typeof(string))]
        [JsonProperty("Created")]
        public DateTimeOffset? Created { get; set; }

        [JsonSchemaType(typeof(string))]
        [JsonProperty("Updated")]
        public DateTimeOffset? Updated { get; set; }
    }
}
