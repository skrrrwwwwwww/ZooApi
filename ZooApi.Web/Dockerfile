# Рантайм-образ обязан совпадать с TargetFramework проектов (net9.0), а не с SDK в global.json:
# .NET по умолчанию не перескакивает через мажорную версию, и aspnet:10.0 не запустит net9.0-сборку.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# SDK 10 — чтобы удовлетворить global.json (SDK 10.0.0); он умеет собирать под net9.0.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Слой восстановления: копируем только манифесты проектов и Directory.Packages.props,
# иначе Central Package Management не найдёт версии (NU1015).
COPY ["Directory.Packages.props", "."]
COPY ["ZooApi.Domain/ZooApi.Domain.csproj", "ZooApi.Domain/"]
COPY ["ZooApi.Application/ZooApi.Application.csproj", "ZooApi.Application/"]
COPY ["ZooApi.Infrastructure/ZooApi.Infrastructure.csproj", "ZooApi.Infrastructure/"]
COPY ["ZooApi.Web/ZooApi.Web.csproj", "ZooApi.Web/"]
RUN dotnet restore "ZooApi.Web/ZooApi.Web.csproj"

COPY . .
RUN dotnet build "./ZooApi.Web/ZooApi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ZooApi.Web/ZooApi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZooApi.Web.dll"]
