namespace Nebulift.Api.Templates;

using Types;
using Nebulift.Api.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text.Json;

/// <summary>
/// A local repository implementation for accessing template-related data stored in the file system.
/// </summary>
public class TemplateLocalRepository : ITemplateRepository
{
    private readonly string _templatesFolderPath;
    private readonly ILogger<TemplateLocalRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateLocalRepository"/> class with a specified templates folder.
    /// </summary>
    /// <param name="options">The options containing the folder path where template data is stored.</param>
    /// <param name="logger">An instance of <see cref="ILogger{TemplateLocalRepository}"/> for logging.</param>
    public TemplateLocalRepository(IOptions<TemplateOptions> options, ILogger<TemplateLocalRepository> logger)
    {
        _templatesFolderPath = options.Value.TemplatesFolderPath;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Retrieves the identities of all templates.
    /// </summary>
    /// <returns>The identities of the templates as a list of <see cref="Identity"/>.</returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown if any of the identity files are not found at the expected paths.
    /// </exception>
    public List<Identity> GetAllTemplateIdentities()
    {
        var templateIdentities = new List<Identity>();
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
                var identity = JsonSerializer.Deserialize<Identity>(jsonContent);
                templateIdentities.Add(identity);
                _logger.LogInformation("Successfully serialized identity into {Identity}", identity);
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
    /// <returns>The identity of the template as a <see cref="Identity"/>.</returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown if the identity file is not found at the expected path.
    /// </exception>
    public Identity GetTemplateIdentity(string id)
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
            var identity = JsonSerializer.Deserialize<Identity>(jsonContent);
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
    /// <returns>The inputs of the template as a <see cref="Inputs"/>.</returns>
    public Inputs GetTemplateInputs(string id)
    {
        _logger.LogInformation("Fetching template inputs for template ID: {TemplateId}", id);

        // Placeholder implementation
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves the outputs of a template by its ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The outputs of the template as a <see cref="Outputs"/>.</returns>
    public Outputs GetTemplateOutputs(string id)
    {
        _logger.LogInformation("Fetching template outputs for template ID: {TemplateId}", id);

        // Placeholder implementation
        throw new NotImplementedException();
    }
}