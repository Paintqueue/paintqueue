using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PainterQueueApi.Models
{
    /// <summary>
    /// Represents a user entity with unique identification and associated details.
    /// </summary>
    /// <remarks>The <see cref="User"/> class is designed to store information about a user, including their
    /// unique identifier, object ID, username, and email address. The <see cref="ObjectId"/> property serves as the foriegn
    /// with the identity service.</remarks>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the object with the identiy service.
        /// </summary>
        [Required]
        public Guid ObjectId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
