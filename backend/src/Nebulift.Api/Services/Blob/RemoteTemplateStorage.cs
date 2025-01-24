namespace Nebulift.Api.Services.Blob;

using System.Collections.Concurrent;
using Types;
using Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Xml;

/// <summary>
/// A remote template storage implementation for accessing templates stored in the Nebulift project repository.
/// </summary>
public sealed class RemoteTemplateStorage : ITemplateStorage, IDisposable
{
    private record TemplateData(TemplateIdentity identity, TemplateInputs inputs, TemplateCodeReference coderef, TemplateOutputs outputs);

    private readonly ILogger<RemoteTemplateStorage> _logger;
    private readonly ConcurrentDictionary<string, TemplateData> _templatesData = new ();
    private readonly Uri _rootUrl;
    private readonly HttpClient _client = new ();
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteTemplateStorage"/> class with a specified 'templates' folder.
    /// </summary>
    /// <param name="options">The options containing the folder path where template data is stored.</param>
    /// <param name="logger">An instance of <see cref="ILogger{RemoteTemplateStorage}"/> for logging.</param>
    public RemoteTemplateStorage(IOptions<RemoteTemplateServiceOptions> options, ILogger<RemoteTemplateStorage> logger)
    {
        string rootUrl = options == null
            ? throw new ArgumentNullException(nameof(options))
            : options.Value.TemplatesRootUrl ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rootUrl = new Uri(rootUrl.TrimEnd('/'));

        // Fetch everything directly, avoid making multiple requests
        _logger.LogInformation("Fetching all template data from URL: {RootUrl}", _rootUrl);

        _ = InitData();
    }

    /// <summary>
    /// Retrieves the coderef of a given template.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The coderef of the template as a <see cref="TemplateCodeReference"/>.</returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown if any of the coderef file is not found at the expected paths.
    /// </exception>
    public TemplateCodeReference GetTemplateCodeReference(string id)
    {
        if (!_templatesData.TryGetValue(id, out TemplateData? tdata))
        {
            _logger.LogError("Template data of {Id} not found", id);
            throw new FileNotFoundException($"Template data of {id} not found");
        }

        if (tdata == null)
        {
            _logger.LogError("Template data of {Id} is null", id);
            throw new FileNotFoundException($"Template data of {id} is null");
        }

        return tdata.coderef;
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
    /// Retrieves the outputs of a template by its ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The outputs of the template as a <see cref="TemplateOutputs"/>.</returns>
    public TemplateOutputs GetTemplateOutputs(string id)
    {
        _logger.LogInformation("Retrieving template inputs for template ID: {TemplateId}", id);

        if (!_templatesData.TryGetValue(id, out TemplateData? data))
        {
            throw new FileNotFoundException($"Inputs {id} not found");
        }

        return data.outputs;
    }

    /// <summary>
    /// Cleans up the resources used by the <see cref="RemoteTemplateStorage"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
    }

    private async Task InitData()
    {
        var listUri = new Uri(_rootUrl, $"{_rootUrl.AbsolutePath.TrimEnd('/')}/?comp=list&delimiter=/");

        _logger.LogInformation("Fetching template list from URL: {ListUri}", listUri);
        HttpResponseMessage response;
        try
        {
            response = await _client.GetAsync(listUri);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching templates list from {ListUri}", listUri);
            throw;
        }

        var content = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Received template list content: {Content}", content);

        var doc = new XmlDocument();
        try
        {
            doc.LoadXml(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing XML content from {ListUri}", listUri);
            throw;
        }

        XmlNodeList blobs = doc.GetElementsByTagName("Name");
        foreach (XmlNode template in blobs)
        {
            string templateName = template.InnerText.TrimEnd('/');
            _logger.LogInformation("Found template: {BlobName}", templateName);

            try
            {
                var identityFile = await BlobFileReader.ParseFile(templateName, _rootUrl, "identity.json");
                var inputsFile = await BlobFileReader.ParseFile(templateName, _rootUrl, "inputs.json");
                var refFile = await BlobFileReader.ParseFile(templateName, _rootUrl, "coderef.json");
                var outputsFile = await BlobFileReader.ParseFile(templateName, _rootUrl, "outputs.json");

                var identity = BlobFileReader.ParseIdentity(identityFile);
                var inputs = BlobFileReader.ParseInputs(inputsFile);
                var codeReference = BlobFileReader.ParseCodeReference(refFile);
                var outputs = BlobFileReader.ParseOutputs(outputsFile);

                if (!_templatesData.TryAdd(identity.Name, new TemplateData(identity, inputs, codeReference, outputs)))
                {
                    _logger.LogWarning("Storage: Template {TemplateId} already exists and was not overwritten.", identity.Name);
                }
                else
                {
                    _logger.LogInformation("Storage: Successfully loaded template {TemplateId}", templateName);
                }
            }
            catch (FileNotFoundException e)
            {
                _logger.LogWarning(e, "Template {TemplateId} does not have all required files", templateName);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(e, "Error parsing template {TemplateId}", templateName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error while processing template {TemplateId}", templateName);
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