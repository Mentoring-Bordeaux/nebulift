namespace Nebulift.Api.Models;

/// <summary>
/// Represents a weather forecast with a date, temperature in Celsius, and an optional summary.
/// </summary>
/// <param name="date">The date of the weather forecast.</param>
/// <param name="temperatureC">The temperature in Celsius.</param>
/// <param name="summary">An optional summary of the weather forecast.</param>
public record WeatherForecast(DateOnly date, int temperatureC, string? summary)
{
    /// <summary>
    /// Gets the temperature in Fahrenheit.
    /// </summary>
    public int TemperatureF => 32 + (int)(temperatureC / 0.5556);
}
