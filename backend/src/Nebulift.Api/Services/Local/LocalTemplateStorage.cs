namespace Nebulift.Api.Services.Local;

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
public class LocalTemplateStorage : ITemplateStorage
{
    private readonly string _templatesFolderPath;
    private readonly ILogger<LocalTemplateStorage> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalTemplateStorage"/> class with a specified 'templates' folder.
    /// </summary>
    /// <param name="options">The options containing the folder path where template data is stored.</param>
    /// <param name="logger">An instance of <see cref="ILogger{LocalTemplateService}"/> for logging.</param>
    public LocalTemplateStorage(IOptions<LocalTemplateServiceOptions> options, ILogger<LocalTemplateStorage> logger)
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

    /// <summary>
    /// Retrieves the outputs of a template by its ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The outputs of the template as a <see cref="TemplateOutputs"/>.</returns>
    public TemplateOutputs GetTemplateOutputs(string id)
    {
        var outputsFilePath = Path.Combine(_templatesFolderPath, id, "outputs.json");
        _logger.LogInformation("Fetching template outputs for template ID: {TemplateId} from path: {OutputsFilePath}", id, outputsFilePath);

        try
        {
            if (!File.Exists(outputsFilePath))
            {
                _logger.LogWarning("Outputs file not found for template ID {TemplateId} at path: {OutputsFilePath}", id, outputsFilePath);
                throw new FileNotFoundException($"Outputs file not found at path: {outputsFilePath}");
            }

            var jsonContent = JsonNode.Parse(File.ReadAllText(outputsFilePath));
            if (jsonContent == null)
            {
                throw new IOException($"An error occurred while parsing {jsonContent} to output schema");
            }

            var outputs = new TemplateOutputs(jsonContent.AsObject());
            _logger.LogInformation("Successfully retrieved outputs {Outputs}", outputs);
            return outputs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching template outputs for template ID: {TemplateId}", id);
            throw new IOException($"An error occurred while fetching template outputs for template ID: {id}.", ex);
        }
    }
}