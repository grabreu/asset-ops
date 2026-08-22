FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Build.props Directory.Packages.props ./
COPY src/AssetOps.Api/AssetOps.Api.csproj src/AssetOps.Api/
COPY src/AssetOps.Application/AssetOps.Application.csproj src/AssetOps.Application/
COPY src/AssetOps.Domain/AssetOps.Domain.csproj src/AssetOps.Domain/
COPY src/AssetOps.Infrastructure/AssetOps.Infrastructure.csproj src/AssetOps.Infrastructure/
COPY src/AssetOps.ServiceDefaults/AssetOps.ServiceDefaults.csproj src/AssetOps.ServiceDefaults/

RUN dotnet restore src/AssetOps.Api/AssetOps.Api.csproj

COPY src/ src/

RUN dotnet publish src/AssetOps.Api/AssetOps.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["dotnet", "AssetOps.Api.dll"]
