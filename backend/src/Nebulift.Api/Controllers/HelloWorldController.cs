namespace Nebulift.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Nebulift.Api.Templates;

/// <summary>
/// Controller to handle Nebulift template requests
/// </summary>
[ApiController]
[Route("api/templates")]
public class TemplateController : ControllerBase
{
    /// <summary>
    /// Retrieves all templates.
    /// </summary>
    [HttpGet]
    public IActionResult GetAllTemplates()
    {
        // Example response: list of templates
        var templates = new List<string> { "Template1", "Template2", "Template3" };
        return Ok(templates);
    }

    /// <summary>
    /// Retrieves a specific template by ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    [HttpGet("{id}")]
    public IActionResult GetTemplateById(string id)
    {
        // Example: fetch a specific template based on ID
        var template = new { Id = id, Name = $"Template-{id}" };
        return Ok(template);
    }

    /// <summary>
    /// Execute a specific template by ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <param name="templateData">The template data to update.</param>
    [HttpPost("{id}")]
    public IActionResult ExecuteTemplateById(string id, [FromBody] object templateData)
    {
        // Example: update the template and return the updated version
        var updatedTemplate = new { Id = id, Data = templateData };
        return Ok(updatedTemplate);
    }
}
