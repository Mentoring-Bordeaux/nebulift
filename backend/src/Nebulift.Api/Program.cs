namespace Nebulift.Api;

using Middleware;
using Nebulift.Api.Templates;
using Nebulift.Api.Configuration;

/// <summary>
/// Main program for Nebulift backend.
/// </summary>
public static class Program
{
    /// <summary>
    /// Main program for Nebulift backend.
    /// </summary>
    /// <param name="args">Arguments of the main program.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.Configure<LocalTemplateServiceOptions>(builder.Configuration.GetSection("LocalTemplateServiceOptions"));
        builder.Services.AddScoped<ITemplateService, LocalTemplateService>();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddTransient<ExceptionMiddleware>();

        // Add logging services (ILogger)
        builder.Services.AddLogging();

        // Add CORS services
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowSpecificOrigin",
                builder => builder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        builder.Services.AddHealthChecks();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Use CORS middleware
        // app.UseCors("AllowSpecificOrigin");

        app.UseHealthChecks("/api/health");

        // Use custom exception middleware
        app.UseMiddleware<ExceptionMiddleware>();

        app.MapControllers();

        app.Run();
    }
}