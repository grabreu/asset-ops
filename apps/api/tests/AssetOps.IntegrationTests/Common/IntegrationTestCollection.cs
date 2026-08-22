namespace AssetOps.IntegrationTests.Common;

[CollectionDefinition(Name)]
public sealed class IntegrationTestCollection : ICollectionFixture<ApiFactory>
{
    public const string Name = "Integration";
}
