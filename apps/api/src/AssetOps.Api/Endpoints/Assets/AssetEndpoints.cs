using AssetOps.Application.Commands.Assets.Create;
using AssetOps.Application.Models.Assets;
using AssetOps.Application.Queries.Assets.List;

namespace AssetOps.Api.Endpoints.Assets;

public static class AssetEndpoints
{
    public static IEndpointRouteBuilder MapAssetEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/assets")
            .WithTags("Assets");

        group.MapPost("/", CreateAssetAsync)
            .WithName("CreateAsset")
            .Produces<AssetSummaryDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);

        group.MapGet("/", ListAssetsAsync)
            .WithName("ListAssets")
            .Produces<List<AssetSummaryDto>>(StatusCodes.Status200OK);

        return app;
    }

    private static async Task<IResult> CreateAssetAsync(CreateAssetRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new CreateAssetCommand(request.Tag, request.Name);
        var result = await sender.Send(command, cancellationToken);
        return result.ToCreated(asset => $"/assets/{asset.Id}");
    }

    private static async Task<IResult> ListAssetsAsync(ISender sender, CancellationToken cancellationToken)
    {
        var query = new ListAssetsQuery();
        var result = await sender.Send(query, cancellationToken);
        return result.ToOk();
    }
}
