namespace Nebulift.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Nebulift.Api.Templates;

/// <summary>
/// Controller to handle requests for the HelloWorld template.
/// TODO: Move to controllers folder. 
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HelloWorldController : ControllerBase
{
   [HttpGet]
   public IActionResult Get()
   {
       var helloWorld = new HelloWorld("TEST-API-KEY");
       return Ok(helloWorld);
   }
}
