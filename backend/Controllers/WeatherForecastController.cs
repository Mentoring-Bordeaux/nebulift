namespace Nebulift.Controllers;

using Microsoft.AspNetCore.Mvc;
using Nebulift.Models;

/// <summary>
/// Controller to handle weather forecast related requests.
/// </summary>
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] _summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
    };

    /// <summary>
    /// Gets the weather forecast for the next 5 days.
    /// </summary>
    /// <returns>An enumerable of <see cref="WeatherForecast"/> objects.</returns>
    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                _summaries[Random.Shared.Next(_summaries.Length)]))
            .ToArray();
    }
}