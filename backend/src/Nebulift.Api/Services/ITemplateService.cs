namespace Nebulift.Api.Services;

/// <summary>
/// Macro interface for managing templates (data + execution).
/// </summary>
public interface ITemplateService : ITemplateStorage, ITemplateExecutor
{
}