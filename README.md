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

.NET 10 / C# 14, ASP.NET Core, EF Core with PostgreSQL, MassTransit with RabbitMQ, Redis, FluentValidation, AutoMapper, and MailKit.

## Run with Docker Compose

1. Copy the environment template and set `MAIL_USERNAME` and `MAIL_PASSWORD` to valid SMTP credentials if email notifications are needed:

   ```bash
   cp backend/ZooApi.Web/.env.example backend/ZooApi.Web/.env
   ```

2. Start the API and its dependencies:

   ```bash
   docker compose --env-file backend/ZooApi.Web/.env up --build
   ```

The API is available at `http://localhost:8080`. Health check: `/health`; OpenAPI JSON: `/openapi/v1.json`; Swagger UI: `/swagger`.

The local `.env` file is ignored by Git. Do not commit real credentials.

## Run the API locally

Start PostgreSQL, RabbitMQ, and Redis, then run:

```bash
dotnet run --project backend/ZooApi.Web/ZooApi.Web.csproj
```

The default development connection settings are in `backend/ZooApi.Web/appsettings.Development.json`.

## API routes

Routes use the `api` prefix:

- `GET /api/Animals` and `GET /api/Owners` — paginated lists (`pageNumber`, `pageSize`).
- `GET` and `DELETE /api/Animals/{id}`; `POST /api/Animals` — retrieve, delete, and create an animal.
- `PUT /api/Animals/{id}/feed` and `PUT /api/Animals/{id}/play` — change an animal's state.
- `GET`, `PUT`, and `DELETE /api/Owners/{id}`; `POST /api/Owners` — retrieve, update, delete, and create an owner.

Run `dotnet build backend/ZooApi.slnx --configuration Release` to build the backend solution.
