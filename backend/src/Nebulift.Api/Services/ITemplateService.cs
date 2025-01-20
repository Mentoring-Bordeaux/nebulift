namespace Nebulift.Api.Services;

using Templates;

/// <summary>
/// Macro interface for managing templates (data + execution).
/// </summary>
public interface ITemplateService : ITemplateStorage, ITemplateExecutor
{
}