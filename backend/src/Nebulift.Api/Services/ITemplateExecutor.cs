namespace Nebulift.Api.Services;

using Types;

/// <summary>
/// Executing interface for running template codes.
/// </summary>
public interface ITemplateExecutor
{
    /// <summary>
    /// Execute a template and returns its outputs.
    /// </summary>
    /// <param name="id">The id of the template to execute.</param>
    /// <param name="inputs">The parameters to execute the template with (must comply to its inputs schema).</param>
    /// <returns>The outputs of the template as a <see cref="TemplateOutputs"/>.</returns>
    public Task<TemplateOutputs?> ExecuteTemplate(string id, TemplateInputs inputs);
}