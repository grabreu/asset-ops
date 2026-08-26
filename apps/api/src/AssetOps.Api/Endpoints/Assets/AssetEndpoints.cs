using AssetOps.Application.Models.Assets;
using AssetOps.Application.Queries.Assets.List;

namespace AssetOps.Api.Endpoints.Assets;

public static class AssetEndpoints
{
    public static IEndpointRouteBuilder MapAssetEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/assets")
            .WithTags("Assets");

        group.MapGet("/", ListAssetsAsync)
            .WithName("ListAssets")
            .Produces<List<AssetSummaryDto>>(StatusCodes.Status200OK);

        return app;
    }

    private static async Task<IResult> ListAssetsAsync(ISender sender, CancellationToken cancellationToken)
    {
        var query = new ListAssetsQuery();
        var result = await sender.Send(query, cancellationToken);
        return result.ToOk();
    }
}
