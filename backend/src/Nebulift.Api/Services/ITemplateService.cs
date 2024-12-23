namespace Nebulift.Api.Templates;

using Types;

/// <summary>
/// Macro interface for managing templates (data + execution).
/// </summary>
public interface ITemplateService : ITemplateStorage, ITemplateExecutor
{
}