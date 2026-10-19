# Zoo Management API

REST API for managing zoo animals and their owners.

## Architecture

The .NET 10 solution is split into four projects:

- `ZooApi.Domain` — entities and domain behavior.
- `ZooApi.Application` — DTOs, validation, services, and message consumers.
- `ZooApi.Infrastructure` — PostgreSQL persistence, EF Core migrations, RabbitMQ/MassTransit, Redis, and logging.
- `ZooApi.Web` — ASP.NET Core API, controllers, exception handling, and OpenAPI/Swagger UI.

The Vue 3 frontend in `frontend/` is currently a starter template and is not yet connected to the API.

## Features

- Create, list, retrieve, update, and delete owners.
- Create, list, retrieve, and delete animals; feed and play with an animal to update its energy and activity intensity.
- Paginated owner and animal lists.
- Publish owner/animal creation events through the MassTransit EF outbox. Consumers cache event data in Redis and send email notifications.
- PostgreSQL migrations are applied at API startup.

## Stack

.NET 10 / C# 14, ASP.NET Core, EF Core with PostgreSQL, MassTransit with RabbitMQ, Redis, FluentValidation, explicit DTO mapping, and MailKit.

## Run with Docker Compose

1. Copy the environment template, replace the sample database and RabbitMQ passwords, and configure `JWT_AUTHORITY` and `JWT_AUDIENCE` to match your external identity provider. Set SMTP credentials if email notifications are needed:

   ```bash
   cp backend/ZooApi.Web/.env.example backend/ZooApi.Web/.env
   ```

2. Start the API and its dependencies:

   ```bash
   docker compose --env-file backend/ZooApi.Web/.env up --build
   ```

The API is available at `http://localhost:8080` on loopback only. Health check: `/health`; OpenAPI JSON: `/openapi/v1.json`; Swagger UI: `/swagger`. Put a TLS-terminating reverse proxy in front of the API before exposing it to a network.

The local `.env` file is ignored by Git. Do not commit real credentials. The API validates HTTPS JWT issuer metadata and audience; it does not issue tokens or create users. `/health` and the OpenAPI/Swagger documentation remain public, while owner and animal endpoints require a bearer token.

## Run the API locally

Set local PostgreSQL, RabbitMQ, and identity-provider settings in user secrets, start PostgreSQL/RabbitMQ/Redis, then run:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5433;Database=zoo_db;Username=zoo_user;Password=<your-local-db-password>" --project backend/ZooApi.Web/ZooApi.Web.csproj
dotnet user-secrets set "RabbitMq:Username" "zoo_user" --project backend/ZooApi.Web/ZooApi.Web.csproj
dotnet user-secrets set "RabbitMq:Password" "<your-local-rabbitmq-password>" --project backend/ZooApi.Web/ZooApi.Web.csproj
dotnet user-secrets set "Authentication:Authority" "https://your-identity-provider/" --project backend/ZooApi.Web/ZooApi.Web.csproj
dotnet user-secrets set "Authentication:Audience" "your-api-audience" --project backend/ZooApi.Web/ZooApi.Web.csproj
dotnet run --project backend/ZooApi.Web/ZooApi.Web.csproj
```

Add SMTP settings to user secrets if notifications are needed.

## API routes

Routes use the `api` prefix:

- `GET /api/Animals` and `GET /api/Owners` — paginated lists (`pageNumber`, `pageSize`).
- `GET` and `DELETE /api/Animals/{id}`; `POST /api/Animals` — retrieve, delete, and create an animal.
- `PUT /api/Animals/{id}/feed` and `PUT /api/Animals/{id}/play` — change an animal's state.
- `GET`, `PUT`, and `DELETE /api/Owners/{id}`; `POST /api/Owners` — retrieve, update, delete, and create an owner.

Run `dotnet build ZooApi.slnx --configuration Release` to build the solution.
Run `dotnet test ZooApi.slnx --configuration Release` to execute the automated tests.
