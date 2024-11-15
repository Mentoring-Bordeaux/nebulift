namespace Nebulift.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Nebulift.Api.Templates;
using System.Text.Json.Serialization;

/// <summary>
/// Controller to handle requests for the HelloWorld template.
/// </summary>
[ApiController]
[Route("templates/[controller]")]
public class HelloWorldController : ControllerBase
{
   private static HelloWorld _helloWorld = new HelloWorld();

   [HttpGet]
   public IActionResult Get()
   {
       return Ok(_helloWorld);
   }

   [HttpPost]
   public IActionResult Post([FromBody] ApiKeyRequest request)
   {
      if (request == null || string.IsNullOrEmpty(request.ApiKey))
      {
          return BadRequest("ApiKey is required.");
      }

      _helloWorld.ApiKey = request.ApiKey;
      return Ok(_helloWorld);
   }
}


public class ApiKeyRequest
{
   [JsonPropertyName("apiKey")]
   public string ApiKey { get; set; }
}