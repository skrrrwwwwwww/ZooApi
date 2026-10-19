# Рантайм-образ обязан совпадать с TargetFramework проектов, иначе .NET не переживёт
# несовпадение мажорных версий и процесс не стартует.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# SDK 10 — чтобы удовлетворить global.json (SDK 10.0.0); он же собирает под net10.0.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Слой восстановления: копируем только манифесты проектов и Directory.Packages.props,
# иначе Central Package Management не найдёт версии (NU1015).
# ВЕЗДЕ ДОБАВЛЯЕМ "backend/" В ПУТИ ИСТОЧНИКА:
COPY ["backend/Directory.Packages.props", "."]
COPY ["backend/ZooApi.Domain/ZooApi.Domain.csproj", "ZooApi.Domain/"]
COPY ["backend/ZooApi.Application/ZooApi.Application.csproj", "ZooApi.Application/"]
COPY ["backend/ZooApi.Infrastructure/ZooApi.Infrastructure.csproj", "ZooApi.Infrastructure/"]
COPY ["backend/ZooApi.Web/ZooApi.Web.csproj", "ZooApi.Web/"]

RUN dotnet restore "ZooApi.Web/ZooApi.Web.csproj"

# Копируем оставшиеся исходные коды бэкенда
COPY backend/ .

RUN dotnet build "ZooApi.Web/ZooApi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ZooApi.Web/ZooApi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
# В aspnet-образе нет ни curl, ни wget, а compose healthcheck зовёт curl.
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZooApi.Web.dll"]
