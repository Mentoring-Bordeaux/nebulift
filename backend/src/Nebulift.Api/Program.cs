namespace Nebulift.Api;

using Middleware;
using Templates;
using Configuration;
using Services;
using Services.Blob;

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
        builder.Services.Configure<RemoteTemplateServiceOptions>(
            builder.Configuration.GetSection("RemoteTemplateServiceOptions"));
        builder.Services.AddSingleton<ITemplateStorage, RemoteTemplateStorage>();
        builder.Services.AddScoped<ITemplateExecutor, RemoteTemplateExecutor>();

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
                policy => policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        builder.Services.AddHealthChecks();

        var app = builder.Build();

        // Forcing instantiation of template storage to run the first requests.
        try
        {
            app.Services.GetRequiredService<ITemplateStorage>();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while initializing template storage: " + e.Message);
            return;
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseHealthChecks("/api/health");

        // Use custom exception middleware
        app.UseMiddleware<ExceptionMiddleware>();

        app.MapControllers();

        app.Run();
    }
}