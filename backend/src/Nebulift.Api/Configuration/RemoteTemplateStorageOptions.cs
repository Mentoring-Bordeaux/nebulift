namespace Nebulift.Api.Configuration
{
    /// <summary>
    /// Options for the RemoteTemplateStorage class.
    /// </summary>
    public class RemoteTemplateStorageOptions
    {
        /// <summary>
        /// Gets the url the RemoteTemplateStorage must use to fetch templates.
        /// </summary>
        public string? TemplatesRootUrl { get; init; }
    }
}