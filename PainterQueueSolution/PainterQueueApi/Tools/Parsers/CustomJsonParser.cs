using Microsoft.AspNetCore.Mvc;
using PainterQueueApi.Infrastructure.Interfaces;
using System.Text;
using System.Text.Json;

namespace PainterQueueApi.Tools.Parsers
{
    /// <summary>
    /// Provides functionality to parse JSON data from a stream into objects of a specified type.
    /// </summary>
    /// <remarks>This class is designed to handle JSON streams and deserialize them into strongly-typed
    /// objects. It uses <see cref="JsonSerializer"/> with options configured to allow case-insensitive property names,
    /// skip comments, and handle trailing commas in the JSON data. Errors during parsing are logged using the provided
    /// telemetry client.</remarks>
    public static class CustomJsonParser
    {
        /// <summary>
        /// Asynchronously parses a JSON stream into a list of objects of the specified type.
        /// </summary>
        /// <remarks>The method uses <see cref="JsonSerializer"/> with options configured to allow
        /// case-insensitive  property names, skip comments, and handle trailing commas in the JSON data. If the JSON
        /// format is invalid or a mapping issue occurs, the method logs the error and returns <see
        /// langword="null"/>.</remarks>
        /// <typeparam name="T">The type of objects to deserialize from the JSON stream.</typeparam>
        /// <param name="jsonStream">The stream containing the JSON data to parse. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="jsonStream"/> is <see langword="null"/>.</exception>"
        /// <exception cref="JsonException">Thrown when the JSON format is invalid or a mapping issue occurs.</exception>
        /// <returns>A list of objects of type <typeparamref name="T"/> if the JSON is successfully parsed;  otherwise, <see
        /// langword="null"/> if the JSON is invalid or an error occurs during parsing.</returns>
        public static async Task<List<T>> ParseStreamAsync<T>(Stream jsonStream)
        {
            if (jsonStream == null) throw new ArgumentNullException(nameof(jsonStream));

            return await JsonSerializer.DeserializeAsync<List<T>>(jsonStream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            }) ?? throw new JsonException("Deserialized list was null.");
        }
    }
}
