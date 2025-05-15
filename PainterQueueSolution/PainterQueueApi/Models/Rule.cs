using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PainterQueueApi.Models
{
    /// <summary>
    /// Represents a rule entity with metadata and descriptive information.
    /// </summary>
    /// <remarks>The <see cref="Rule"/> class is designed to store information about a specific Warhammer rule, 
    /// including its unique identifier, name, descriptions, and timestamps for creation and updates. Rules are the
    /// fundamental entity of other entities</remarks>
    public class Rule
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string InternalDescription { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Page { get; set; } = 0;
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
    }
}
