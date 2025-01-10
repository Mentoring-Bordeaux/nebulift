namespace Nebulift.Api.Models
{
    using System.Text.Json;

    /// <summary>
    /// Represents the details of an error.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Gets or sets the status code of the error.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Method toString for the error details.
        /// </summary>
        /// <returns>
        /// A string representation of the error details.
        /// </returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}