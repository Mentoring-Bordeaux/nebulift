namespace Nebulift.Api.Configuration
{
    /// <summary>
    /// Options for the LocalTemplateService class.
    /// </summary>
    public class LocalTemplateServiceOptions
    {
        /// <summary>
        /// Gets or sets the folder path the LocalTemplateService must search into.
        /// </summary>
        public string? TemplatesFolderPath { get; set; }
    }
}