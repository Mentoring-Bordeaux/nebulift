#pragma warning disable CA1506
using Microsoft.Extensions.ServiceDiscovery;
using Nebulift.Api.Configuration;
using Nebulift.Api.Middleware;
using Nebulift.Api.Services;
using Nebulift.Api.Services.Blob;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integration
builder.AddServiceDefaults();

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
    if (builder.Configuration["FrontUrl"] is { } frontUrl)
    {
        app.UseCors(policyBuilder =>
        {
            policyBuilder.WithOrigins(frontUrl)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Use custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
#pragma warning restore CA1506
