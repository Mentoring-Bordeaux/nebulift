namespace Nebulift.Api.Services;

using Types;
using Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;

/// <summary>
/// A remote template storage implementation for accessing templates stored in the Nebulift project repository.
/// </summary>
public sealed class RemoteTemplateStorage : ITemplateStorage, IDisposable
{
    private readonly ILogger<RemoteTemplateStorage> _logger;
    private readonly Dictionary<string, TemplateData> _templatesData = new ();
    private readonly Uri _rootUrl;
    private readonly HttpClient _client = new ();
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteTemplateStorage"/> class with a specified 'templates' folder.
    /// </summary>
    /// <param name="options">The options containing the folder path where template data is stored.</param>
    /// <param name="logger">An instance of <see cref="ILogger{RemoteTemplateStorage}"/> for logging.</param>
    public RemoteTemplateStorage(IOptions<RemoteTemplateStorageOptions> options, ILogger<RemoteTemplateStorage> logger)
    {
        string rootUrl = options == null
            ? throw new ArgumentNullException(nameof(options))
            : options.Value.TemplatesRootUrl ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rootUrl = new Uri(rootUrl.TrimEnd('/'));

        // Fetch everything directly, avoid making multiple requests
        _logger.LogInformation("Fetching all template data from URL: {RootUrl}", _rootUrl);
        InitData();
    }

    /// <summary>
    /// Retrieves the identities of all templates.
    /// </summary>
    /// <returns>The identities of the templates as a list of <see cref="TemplateIdentity"/>.</returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown if any of the identity files are not found at the expected paths.
    /// </exception>
    public List<TemplateIdentity> GetAllTemplateIdentities()
    {
        _logger.LogInformation("Retrieving all template identities");
        return _templatesData.Values.Select(data => data.identity).ToList();
    }

    /// <summary>
    /// Retrieves the identity of a template by its ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The identity of the template as a <see cref="TemplateIdentity"/>.</returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown if the identity file is not found at the expected path.
    /// </exception>
    public TemplateIdentity GetTemplateIdentity(string id)
    {
        _logger.LogInformation("Retrieving template identity for template ID: {TemplateId}", id);

        if (!_templatesData.TryGetValue(id, out TemplateData? data))
        {
            throw new FileNotFoundException($"Identity {id} not found");
        }

        return data.identity;
    }

    /// <summary>
    /// Retrieves the inputs of a template by its ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The inputs of the template as a <see cref="TemplateInputs"/>.</returns>
    public TemplateInputs GetTemplateInputs(string id)
    {
        _logger.LogInformation("Retrieving template inputs for template ID: {TemplateId}", id);

        if (!_templatesData.TryGetValue(id, out TemplateData? data))
        {
            throw new FileNotFoundException($"Inputs {id} not found");
        }

        return data.inputs;
    }

    /// <summary>
    /// Cleans up the resources used by the <see cref="RemoteTemplateStorage"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
    }

    private static TemplateIdentity ParseIdentity(JsonElement element)
    {
        var name = element.GetProperty("name").GetString() ?? throw new ArgumentNullException(nameof(element));
        var url = element.GetProperty("url").GetString() ?? throw new ArgumentNullException(nameof(element));
        var path = element.GetProperty("path").GetString() ?? throw new ArgumentNullException(nameof(element));
        var branch = element.GetProperty("branch").GetString() ?? throw new ArgumentNullException(nameof(element));
        var technologies = element.GetProperty("technologies").EnumerateArray().Select(x => x.ToString()).ToList();

        return new TemplateIdentity(name, url, path, branch, technologies);
    }

    private static TemplateInputs ParseInputs(JsonElement element)
    {
        var jsonObject = JsonNode.Parse(element.GetRawText()) as JsonObject ??
                         throw new ArgumentException("Invalid JSON element");
        return new TemplateInputs(jsonObject);
    }

    private async Task<JsonElement> ParseFile(string templateName, string fileUrl)
    {
        var url = new Uri(_rootUrl, $"{_rootUrl.AbsolutePath.TrimEnd('/')}/{templateName}/{fileUrl}");
        try
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            JsonDocument doc = JsonDocument.Parse(content);

            return doc.RootElement;
        }
        catch (HttpRequestException)
        {
            throw new FileNotFoundException($"File {fileUrl} not found for template {templateName}");
        }
    }

    private async void InitData()
    {
        var listUri = new Uri(_rootUrl, $"{_rootUrl.AbsolutePath.TrimEnd('/')}/?comp=list&delimiter=/");

        var response = await _client.GetAsync(listUri);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var doc = new XmlDocument();
        doc.LoadXml(content);
        XmlNodeList blobs = doc.GetElementsByTagName("Name");
        foreach (XmlNode template in blobs)
        {
            string templateName = template.InnerText.TrimEnd('/');
            _logger.LogInformation("Found template: {BlobName}", templateName);

            try
            {
                var identityFile = await ParseFile(templateName, "identity.json");
                var inputsFile = await ParseFile(templateName, "inputs.json");

                var identity = ParseIdentity(identityFile);
                var inputs = ParseInputs(inputsFile);

                _templatesData.Add(identity.Name, new TemplateData(identity, inputs));
                _logger.LogInformation("Successfully parsed template {TemplateId}", templateName);
            }
            catch (FileNotFoundException e)
            {
                _logger.LogWarning(e, "Template {TemplateId} does not have all required files", templateName);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(e, "Error parsing template {TemplateId}", templateName);
            }
        }
    }

    /// <summary>
    /// Necessary for implementing the <see cref="IDisposable"/> interface.
    /// </summary>
    /// <param name="disposing">Whether the object is being disposed.</param>
    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _client.Dispose();
        }

        _disposed = true;
    }
}