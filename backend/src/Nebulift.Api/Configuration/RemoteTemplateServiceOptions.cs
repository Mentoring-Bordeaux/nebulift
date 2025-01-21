namespace Nebulift.Api.Configuration
{
    /// <summary>
    /// Options for the RemoteTemplateService class.
    /// </summary>
    public class RemoteTemplateServiceOptions
    {
        /// <summary>
        /// Gets the url the RemoteTemplateService must use to fetch templates.
        /// </summary>
        public string? TemplatesRootUrl { get; init; }
    }
}