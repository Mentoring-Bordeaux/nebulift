namespace Nebulift.Api.Templates;

using Types;

/// <summary>
/// Repository interface for accessing template-related data.
/// </summary>
public interface ITemplateRepository
{
    /// <summary>
    /// Retrieves the identities of all templates.
    /// </summary>
    /// <returns>The identities of the templates as a list of <see cref="Identity"/>.</returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown if any of the identity files are not found at the expected paths.
    /// </exception>
    public List<Identity> GetAllTemplateIdentities();

    /// <summary>
    /// Retrieves the identity of a template by its ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The identity of the template as a <see cref="Identity"/>.</returns>
    public Identity GetTemplateIdentity(string id);

    /// <summary>
    /// Retrieves the inputs of a template by its ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The inputs of the template as a <see cref="Inputs"/>.</returns>
    public Inputs GetTemplateInputs(string id);

    /// <summary>
    /// Retrieves the outputs of a template by its ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>The outputs of the template as a <see cref="Outputs"/>.</returns>
    public Outputs GetTemplateOutputs(string id);
}
