var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("AssetOpsDb");

builder.AddProject<Projects.AssetOps_Api>("api")
    .WithReference(db)
    .WaitFor(db);

await builder.Build().RunAsync();
