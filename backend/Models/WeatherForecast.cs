namespace Nebulift.Models;

public record WeatherForecast(DateOnly date, int temperatureC, string? summary)
{
    public int TemperatureF => 32 + (int)(temperatureC / 0.5556);
};