namespace Nebulift.Api;
using Nebulift.Api.Templates;
using Nebulift.Api.Configuration;
using EnvironmentName = Microsoft.Extensions.Hosting.EnvironmentName;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.Configure<TemplateOptions>(builder.Configuration.GetSection("TemplateOptions"));
        builder.Services.AddScoped<ITemplateRepository, TemplateLocalRepository>();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add logging services (ILogger)
        builder.Services.AddLogging();

        // Add CORS services
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
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