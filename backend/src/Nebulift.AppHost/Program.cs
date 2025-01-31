var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Nebulift_Api>("api");

var front = builder.AddPnpmApp("front", "../../../frontend", "dev")
    .WithPnpmPackageInstallation()
    .WithHttpEndpoint(env: "PORT", port: 3000)
    .WithExternalHttpEndpoints()
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();