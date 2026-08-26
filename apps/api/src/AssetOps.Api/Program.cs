using AssetOps.Api.Endpoints.Assets;
using AssetOps.Api.ExceptionHandling;
using AssetOps.Application;
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

builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors();

app.MapOpenApi();
app.MapScalarApiReference();
app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.MapAssetEndpoints();

await app.Services.InitializeDatabaseAsync();

await app.RunAsync();
