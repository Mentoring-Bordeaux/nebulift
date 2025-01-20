namespace Nebulift.Api.Controllers;

using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Templates;
using Types;

/// <summary>
/// Controller to handle Nebulift template requests.
/// </summary>
[ApiController]
[Route("api/templates")]
public class TemplateController : ControllerBase
{
    private readonly ITemplateExecutor _executor;
    private readonly ITemplateStorage _storage;
    private readonly ILogger<TemplateController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateController"/> class.
    /// </summary>
    /// <param name="executor">An instance of <see cref="ITemplateExecutor"/> to handle template execution.</param>
    /// <param name="storage">An instance of <see cref="ITemplateStorage"/> to handle template data retrieval.</param>
    /// <param name="logger">An instance of <see cref="ILogger{TemplateController}"/> for logging.</param>
    public TemplateController(ITemplateExecutor executor, ITemplateStorage storage, ILogger<TemplateController> logger)
    {
        _executor = executor ?? throw new ArgumentNullException(nameof(executor));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
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
        var templates = _storage.GetAllTemplateIdentities();
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
        var templateInputsWrapped = _storage.GetTemplateInputs(id);
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

        var templateIdentity = _storage.GetTemplateIdentity(id);
        var templateInputs = new TemplateInputs(contentObject);

        var templateOutputs = await _executor.ExecuteTemplate(templateIdentity, templateInputs);

        if (templateOutputs == null)
        {
            return BadRequest("Failed to execute template.");
        }

        return Ok(templateOutputs.ToString());
    }
}