namespace Nebulift.Api.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Nebulift.Api.Templates;
    using Nebulift.Api.Types;

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
            try
            {  
               // TODO: select the right template based on the id
                var contentJson = JsonSerializer.Serialize(templateData);

                var contentObject = JsonSerializer.Deserialize<JsonObject>(contentJson);
                var templateInputs = new TemplateInputs(contentObject); // ça sert à rien encore pour le moment
                
                var contentWithoutKey = contentObject["Content"];
                var inputsJson = JsonSerializer.Serialize(contentWithoutKey);
                Console.WriteLine("Inputs: " + inputsJson);
                
                // TODO: set the github token with a value from the inputs ?!
                // TODO: where do we get the pulumi user from ?!
                var envVars = new Dictionary<string, string>
                {
                    { "NEBULIFT_INPUTS", inputsJson },
                    { "GITHUB_TOKEN", "" }, // Add token to test
                    { "PULUMI_USER", "" } // Add user to test 
                };

                _logger.LogInformation("Executing template {TemplateId} with inputs: {Inputs}", id, inputsJson);

                var genProgramPath = @"../Nebulift.Generation";

                // Create a ProcessStartInfo to executre the dotnet run command
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "run",
                    WorkingDirectory = genProgramPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                foreach (var envVar in envVars)
                {
                    processStartInfo.EnvironmentVariables[envVar.Key] = envVar.Value;
                }

                using (var process = Process.Start(processStartInfo))
                {
                    if (process == null)
                    {
                        return StatusCode(500, "Error in the generation process.");
                    }

                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();

                    await process.WaitForExitAsync();

                    if (process.ExitCode != 0)
                    {
                        return StatusCode(500, $"Error in the execution of the C# pulumi program  : {error}");
                    }

                    return Ok(new { Output = output, Error = error });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while executing template : ", ex.Message);
                return StatusCode(500, $"Internal error : {ex.Message}");
            }
        }
    }
}
