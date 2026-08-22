var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AssetOps_Api>("api");

await builder.Build().RunAsync();
