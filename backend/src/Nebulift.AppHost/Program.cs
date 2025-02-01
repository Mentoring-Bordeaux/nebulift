using Nebulift.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Nebulift_Api>("api");

var front = builder.AddViteApp("front", "../../../frontend", "pnpm")
    .WithPnpmPackageInstallation()
    .WithReference(api)
    .WithEnvironment("API_URL", api.GetEndpoint("http"))
    .WithEnvironmentPrefix("NUXT_PUBLIC_")
    .WaitFor(api);

api.WithReference(front)
   .WithEnvironment("FrontUrl", front.GetEndpoint("http"));

builder.Build().Run();