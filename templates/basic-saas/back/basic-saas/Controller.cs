using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [Route("api")]
    [ApiController]
    public class Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Je suis une réponse de l'API !");
        }
    }
}
