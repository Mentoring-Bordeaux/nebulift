namespace Nebulift.Api.Templates;

using Types;
using System.IO;
using System.Text.Json;

/// <summary>
/// A local repository implementation for accessing template-related data stored in the file system.
/// </summary>
public class TemplateLocalRepository : ITemplateRepository
{
    /// <summary>
    /// The folder path where template data is stored.
    /// </summary>
    private readonly string templatesFolder;

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateLocalRepository"/> class with a specified templates folder.
    /// </summary>
    /// <param name="templatesFolder">The folder path where template data is stored.</param>
    public TemplateLocalRepository(string templatesFolder)
    {
        this.templatesFolder = templatesFolder ?? throw new ArgumentNullException(nameof(templatesFolder), "Templates folder cannot be null.");
    }

    /// <summary>
    /// Retrieves the identities of all templates.
    /// </summary>
    /// <returns>The identities of the templates as a list of <see cref="TIdentity"/>.</returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown if any of the identity files are not found at the expected paths.
    /// </exception>
    public List<TIdentity> GetAllTemplateIdentities()
    {
        var templateIdentities = new List<TIdentity>();

        var templateDirs = Directory.GetDirectories(templatesFolder);

        foreach (var templateDir in templateDirs)
        {
            var identityFilePath = Path.Combine(templateDir, "identity.json");

            if (!File.Exists(identityFilePath))
            {
                throw new FileNotFoundException($"Identity file not found at path: {identityFilePath}");
            }

            var jsonContent = File.ReadAllText(identityFilePath);
            var identity = JsonSerializer.Deserialize<TIdentity>(jsonContent);

            templateIdentities.Add(identity);
        }

        return templateIdentities;
    }

    /// <summary>
    /// Retrieves the identity of a template by its ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The identity of the template as a <see cref="TIdentity"/>.</returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown if the identity file is not found at the expected path.
    /// </exception>
    public TIdentity GetTemplateIdentity(string id)
    {
        var identityFilePath = Path.Combine(templatesFolder, id, "identity.json");

        if (!File.Exists(identityFilePath))
        {
            throw new FileNotFoundException($"Identity file not found at path: {identityFilePath}");
        }

        var jsonContent = File.ReadAllText(identityFilePath);
        var identity = JsonSerializer.Deserialize<TIdentity>(jsonContent);

        return identity;
    }

    /// <summary>
    /// Retrieves the inputs of a template by its ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The inputs of the template as a <see cref="TInputs"/>.</returns>
    public TInputs GetTemplateInputs(string id)
    {
        // Placeholder implementation
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves the outputs of a template by its ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The outputs of the template as a <see cref="TOutputs"/>.</returns>
    public TOutputs GetTemplateOutputs(string id)
    {
        // Placeholder implementation
        throw new NotImplementedException();
    }
}
