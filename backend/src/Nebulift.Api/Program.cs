namespace Nebulift.Api;
using Nebulift.Api.Templates;
using Nebulift.Api.Configuration;
using EnvironmentName = Microsoft.Extensions.Hosting.EnvironmentName;

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

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Use CORS middleware
        app.UseCors("AllowSpecificOrigin");

        app.MapControllers();

        app.Run();
    }
}