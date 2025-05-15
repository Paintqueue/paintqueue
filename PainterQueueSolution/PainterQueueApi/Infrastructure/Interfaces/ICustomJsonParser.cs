using System.Text;

namespace PainterQueueApi.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines a contract for parsing JSON data from a stream into a list of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to deserialize from the JSON data.</typeparam>
    public interface ICustomJsonParser<T>
    {
        /// <summary>
        /// Asynchronously parses a JSON stream into a list of objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>The method expects the JSON stream to represent an array of objects of type
        /// <typeparamref name="T"/>. Ensure the stream is properly disposed after calling this method to avoid resource
        /// leaks.</remarks>
        /// <param name="jsonStream">The input stream containing JSON data. Can be <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a list of objects of type
        /// <typeparamref name="T"/> if the stream is successfully parsed, or <see langword="null"/> if the input stream
        /// is <see langword="null"/> or empty.</returns>
        Task<List<T>?> ParseStreamAsync(Stream? jsonStream);
    }
}
