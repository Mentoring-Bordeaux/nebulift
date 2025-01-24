namespace Nebulift.Api.Services.Blob;

using System.Text.Json;
using System.Text.Json.Nodes;
using Types;

/// <summary>
/// Utility class retreiving template data from a blob storage.
/// </summary>
public static class BlobFileReader
{
    /// <summary>
    /// Parses a JSON element into a TemplateInputs object.
    /// </summary>
    /// <param name="element">The JSON element to parse.</param>
    /// <returns>A TemplateInputs object created from the provided JSON.</returns>
    /// <exception cref="ArgumentException">Thrown if the JSON is invalid or cannot be converted to a JsonObject.</exception>
    public static TemplateInputs ParseInputs(JsonElement element)
    {
        var jsonObject = JsonNode.Parse(element.GetRawText()) as JsonObject ??
                         throw new ArgumentException("Invalid JSON element");
        return new TemplateInputs(jsonObject);
    }

    /// <summary>
    /// Parses a JSON element into a TemplateOutputs object.
    /// </summary>
    /// <param name="element">The JSON element to parse.</param>
    /// <returns>A TemplateOutputs object created from the provided JSON.</returns>
    /// <exception cref="ArgumentException">Thrown if the JSON is invalid or cannot be converted to a JsonObject.</exception>
    public static TemplateOutputs ParseOutputs(JsonElement element)
    {
        var jsonObject = JsonNode.Parse(element.GetRawText()) as JsonObject ??
                         throw new ArgumentException("Invalid JSON element");
        return new TemplateOutputs(jsonObject);
    }

    /// <summary>
    /// Parses a JSON element to create a TemplateCodeReference object.
    /// </summary>
    /// <param name="element">The JSON element containing "url", "path", and "branch" properties.</param>
    /// <returns>A TemplateCodeReference object with the extracted data.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any of the required properties are missing or null.</exception>
    public static TemplateCodeReference ParseCodeReference(JsonElement element)
    {
        var url = element.GetProperty("url").GetString() ?? throw new ArgumentNullException(nameof(element));
        var path = element.GetProperty("path").GetString() ?? throw new ArgumentNullException(nameof(element));
        var branch = element.GetProperty("branch").GetString() ?? throw new ArgumentNullException(nameof(element));
        return new TemplateCodeReference(new Uri(url), path, branch);
    }

    /// <summary>
    /// Parses a JSON element to create a TemplateIdentity object.
    /// </summary>
    /// <param name="element">The JSON element containing "name" and "technologies" properties.</param>
    /// <returns>A TemplateIdentity object with the extracted data.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the "name" property is missing or null.</exception>
    public static TemplateIdentity ParseIdentity(JsonElement element)
    {
        var name = element.GetProperty("name").GetString() ?? throw new ArgumentNullException(nameof(element));
        var technologies = element.GetProperty("technologies").EnumerateArray().Select(x => x.ToString()).ToList();
        var description = element.GetProperty("description").GetString() ?? string.Empty;

        return new TemplateIdentity(name, technologies, description);
    }

    /// <summary>
    /// Downloads and parses a JSON file from a given URL.
    /// </summary>
    /// <param name="templateName">The name of the template to which the file belongs.</param>
    /// <param name="rootUrl">The root URL used to construct the full URL.</param>
    /// <param name="fileUrl">The file path relative to the template.</param>
    /// <returns>A JsonElement representing the file's content.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file does not exist or a network error occurs.</exception>
    public static async Task<JsonElement> ParseFile(string templateName, Uri rootUrl, string fileUrl)
    {
        ArgumentNullException.ThrowIfNull(templateName);
        ArgumentNullException.ThrowIfNull(rootUrl);
        ArgumentNullException.ThrowIfNull(fileUrl);

        using (HttpClient client = new ())
        {
            try
            {
                var url = new Uri(rootUrl, $"{rootUrl.AbsolutePath.TrimEnd('/')}/{templateName}/{fileUrl}");

                // Send an HTTP GET request to download the file
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Throws an exception if the HTTP status indicates an error

                // Read and parse the content as JSON
                var content = await response.Content.ReadAsStringAsync();
                JsonDocument doc = JsonDocument.Parse(content);

                return doc.RootElement;
            }
            catch (HttpRequestException)
            {
                // Throw a custom exception if the file is not accessible
                throw new FileNotFoundException($"File {fileUrl} not found for template {templateName}");
            }
        }
    }
}