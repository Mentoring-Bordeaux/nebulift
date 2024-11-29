namespace Nebulift.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Nebulift.Api.Templates;
using System.Text.Json.Serialization;

/// <summary>
/// Controller to handle Nebulift template requests.
/// </summary>
[ApiController]
[Route("api/templates")]
public class TemplateController : ControllerBase
{
    private readonly ITemplateRepository _templateRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateController"/> class.
    /// </summary>
    /// <param name="templateRepository">An instance of <see cref="ITemplateRepository"/> to handle template data retrieval.</param>
    public TemplateController(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository ?? throw new ArgumentNullException(nameof(templateRepository));
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
        return Ok(_templateRepository.GetAllTemplateIdentities());
    }

    /// <summary>
    /// Retrieves a specific template by ID.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <returns>
    /// The template object, containing its ID and Name.
    /// If the template with the specified ID is not found, returns a NotFound result.
    /// </returns>
    [HttpGet("{id}")]
    public IActionResult GetTemplateById(string id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Executes a specific template by ID and updates its data.
    /// </summary>
    /// <param name="id">The ID of the template.</param>
    /// <param name="templateData">The template data to update.</param>
    /// <returns>
    /// The updated template object with the provided data.
    /// </returns>
    [HttpPost("{id}")]
    public IActionResult ExecuteTemplateById(string id, [FromBody] object templateData)
    {
        throw new NotImplementedException();
    }
}
