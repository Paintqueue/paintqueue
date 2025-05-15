using System.Runtime.CompilerServices;

namespace PainterQueueApi.Models
{
    /// <summary>
    /// Provides extension methods for converting between <see cref="User"/> and <see cref="UserDto"/> objects.
    /// </summary>
    /// <remarks>These methods simplify the process of mapping data between the <see cref="User"/> domain
    /// model and the <see cref="UserDto"/> data transfer object.</remarks>
    public static class UserExtensions
    {
        /// <summary>
        /// Converts a <see cref="User"/> object to a <see cref="UserDto"/> object.
        /// </summary>
        /// <param name="user">The <see cref="User"/> instance to convert. Cannot be <see langword="null"/>.</param>
        /// <returns>A <see cref="UserDto"/> object containing the ID, username, and email of the specified <see cref="User"/>.</returns>
        public static UserDto UserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        /// <summary>
        /// Converts a <see cref="UserDto"/> instance to a <see cref="User"/> instance.
        /// </summary>
        /// <param name="userDto">The <see cref="UserDto"/> object to convert. Must not be <c>null</c>.</param>
        /// <returns>A <see cref="User"/> object populated with the data from the specified <see cref="UserDto"/>.</returns>
        public static User User(this UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                Name = userDto.Name ?? string.Empty,
                Email = userDto.Email ?? string.Empty
            };
        }
    }
}
