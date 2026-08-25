using AssetOps.Infrastructure;
using AssetOps.Infrastructure.Persistence;
using AssetOps.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Servers = [];
        return Task.CompletedTask;
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference();
app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

await app.Services.InitializeDatabaseAsync();

await app.RunAsync();
