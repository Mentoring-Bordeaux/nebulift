namespace Nebulift.Api.Services;

using Types;
using Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

/// <summary>
/// A local template service implementation for accessing and running templates stored in the Nebulift project repository.
/// </summary>
public class LocalTemplateService : ITemplateStorage
{
    private readonly string _templatesFolderPath;
    private readonly ILogger<LocalTemplateService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalTemplateService"/> class with a specified 'templates' folder.
    /// </summary>
    /// <param name="options">The options containing the folder path where template data is stored.</param>
    /// <param name="logger">An instance of <see cref="ILogger{LocalTemplateService}"/> for logging.</param>
    public LocalTemplateService(IOptions<LocalTemplateServiceOptions> options, ILogger<LocalTemplateService> logger)
    {
        _templatesFolderPath = options == null
            ? throw new ArgumentNullException(nameof(options))
            : options.Value.TemplatesFolderPath ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        var templateIdentities = new List<TemplateIdentity>();
        _logger.LogInformation("Fetching all template identities from folder: {TemplatesFolder}", _templatesFolderPath);

        try
        {
            var templateDirs = Directory.GetDirectories(_templatesFolderPath);
            foreach (var templateDir in templateDirs)
            {
                var identityFilePath = Path.Combine(templateDir, "identity.json");

                if (!File.Exists(identityFilePath))
                {
                    _logger.LogWarning("Identity file not found at path: {IdentityFilePath}", identityFilePath);
                    continue;
                }

                var jsonContent = File.ReadAllText(identityFilePath);
                _logger.LogInformation("Successfully retrieved identity {Identity}", jsonContent);
                var identity = JsonSerializer.Deserialize<TemplateIdentity>(jsonContent);
                templateIdentities.Add(identity);
                _logger.LogInformation("Successfully serialized identity into {Lbrace}{Identity}{Rbrace}", "{", identity, "}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching template identities.");
            throw new IOException("An error occurred while fetching template identities.", ex);
        }

        return templateIdentities;
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
        var identityFilePath = Path.Combine(_templatesFolderPath, id, "identity.json");
        _logger.LogInformation("Fetching template identity for template ID: {TemplateId} from path: {IdentityFilePath}", id, identityFilePath);

        try
        {
            if (!File.Exists(identityFilePath))
            {
                _logger.LogWarning("Identity file not found for template ID {TemplateId} at path: {IdentityFilePath}", id, identityFilePath);
                throw new FileNotFoundException($"Identity file not found at path: {identityFilePath}");
            }

            var jsonContent = File.ReadAllText(identityFilePath);
            var identity = JsonSerializer.Deserialize<TemplateIdentity>(jsonContent);
            _logger.LogInformation("Successfully retrieved identity {Identity}", identity);
            return identity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching template identity for template ID: {TemplateId}", id);
            throw new IOException($"An error occurred while fetching template identity for template ID: {id}.", ex);
        }
    }

    /// <summary>
    /// Retrieves the inputs of a template by its ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The inputs of the template as a <see cref="TemplateInputs"/>.</returns>
    public TemplateInputs GetTemplateInputs(string id)
    {
        var inputsFilePath = Path.Combine(_templatesFolderPath, id, "inputs.json");
        _logger.LogInformation("Fetching template inputs for template ID: {TemplateId} from path: {InputsFilePath}", id, inputsFilePath);

        try
        {
            if (!File.Exists(inputsFilePath))
            {
                _logger.LogWarning("Inputs file not found for template ID {TemplateId} at path: {InputsFilePath}", id, inputsFilePath);
                throw new FileNotFoundException($"Inputs file not found at path: {inputsFilePath}");
            }

            var jsonContent = JsonNode.Parse(File.ReadAllText(inputsFilePath));
            if (jsonContent == null)
            {
                throw new IOException($"An error occurred while parsing {jsonContent} to input schema");
            }

            var inputs = new TemplateInputs(jsonContent.AsObject());
            _logger.LogInformation("Successfully retrieved inputs {Inputs}", inputs);
            return inputs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching template inputs for template ID: {TemplateId}", id);
            throw new IOException($"An error occurred while fetching template inputs for template ID: {id}.", ex);
        }
    }

    // /// <summary>
    // /// Executes a specific template with parameters.
    // /// </summary>
    // /// <param name="identity">TemplateIdentity of the template to execute.</param>
    // /// <param name="inputs">The template inputs.</param>
    // /// <returns>The outputs of the template as a <see cref="TemplateOutputs"/>.</returns>
    // public Task<TemplateOutputs?> ExecuteTemplate(TemplateIdentity identity, TemplateInputs inputs)
    // {
    //     _logger.LogInformation("Executing template {TemplateId} with parameters {TemplateInputs}", identity, inputs);
    //
    //     // Placeholder implementation
    //     throw new NotImplementedException();
    // }
}