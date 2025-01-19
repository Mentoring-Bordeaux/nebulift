namespace Nebulift.Api;

using Configuration;
using Services;

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
        builder.Services.Configure<RemoteTemplateStorageOptions>(
            builder.Configuration.GetSection("RemoteTemplateStorageOptions"));
        builder.Services.AddSingleton<ITemplateStorage, RemoteTemplateStorage>();
        builder.Services.AddScoped<ITemplateExecutor, RemoteTemplateExecutor>();

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
                policy => policy.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

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

        // Use CORS middleware
        app.UseCors("AllowSpecificOrigin");

        app.MapControllers();

        app.Run();
    }
}