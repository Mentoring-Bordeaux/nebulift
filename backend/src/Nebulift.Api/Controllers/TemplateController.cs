using Nebulift.Api.Services;
using Nebulift.Api.Types;

namespace Nebulift.Api.Controllers
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Controller to handle Nebulift template requests.
    /// </summary>
    [ApiController]
    [Route("api/templates")]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;
        private readonly ILogger<TemplateController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateController"/> class.
        /// </summary>
        /// <param name="templateService">An instance of <see cref="ITemplateService"/> to handle template data retrieval.</param>
        /// <param name="logger">An instance of <see cref="ILogger{TemplateController}"/> for logging.</param>
        public TemplateController(ITemplateService templateService, ILogger<TemplateController> logger)
        {
            _templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves all templates.
        /// </summary>
        /// <returns>
        /// A list of template names.
        /// </returns>
        [HttpGet]
        public IActionResult GetAllTemplates()
        {
            var templates = _templateService.GetAllTemplateIdentities();
            _logger.LogInformation("Retrieving all template identities ({TemplateCount})", templates.Count);
            return Ok(templates);
        }

        /// <summary>
        /// Retrieves a specific template inputs schema by ID.
        /// </summary>
        /// <param name="id">The ID of the template.</param>
        /// <returns>
        /// The template input schema, as a JSON.
        /// If the template with the specified ID is not found, returns a NotFound result.
        /// </returns>
        [HttpGet("{id}")]
        public IActionResult GetTemplateInputsSchemaById(string id)
        {
            var templateInputsWrapped = _templateService.GetTemplateInputs(id);
            _logger.LogInformation("Retrieving template {TemplateId} input schema : {InputSchema}", id, templateInputsWrapped);
            return Ok(templateInputsWrapped.Content);
        }

        /// <summary>
        /// Executes a specific template with parameters.
        /// </summary>
        /// <param name="id">The ID of the template to execute.</param>
        /// <param name="templateData">The template inputs.</param>
        /// <returns>
        /// The updated template object with the provided data.
        /// </returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> ExecuteTemplateById(string id, [FromBody] object templateData)
        {
            var contentJson = JsonSerializer.Serialize(templateData);

            var contentObject = JsonSerializer.Deserialize<JsonObject>(contentJson);
            if (contentObject == null)
            {
                return BadRequest("Invalid JSON object provided.");
            }

            var templateInputs = new TemplateInputs(contentObject);

            await new RemoteTemplateExecutor().ExecuteTemplate(id, templateInputs);
            return Ok();
        }
    }
}