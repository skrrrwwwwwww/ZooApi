.PHONY: build run stop restart db-update migrate clean-cache

# Основные команды для запуска
run:
	docker compose --env-file backend/ZooApi.Web/.env up -d
	dotnet run --project backend/ZooApi.Web/ZooApi.Web.csproj

stop:
	docker compose --env-file backend/ZooApi.Web/.env down

restart: stop run

# Работа с базой данных (EF Core)
# Пример использования: make migrate NAME=AddOwners
migrate:
	dotnet ef migrations add $(NAME) --project backend/ZooApi.Infrastructure/ZooApi.Infrastructure.csproj --startup-project backend/ZooApi.Web/ZooApi.Web.csproj

db-update:
	dotnet ef database update --project backend/ZooApi.Infrastructure/ZooApi.Infrastructure.csproj --startup-project backend/ZooApi.Web/ZooApi.Web.csproj

# Чистка системы от "призраков" и кэша
clean-cache:
	dotnet clean
	find . -type d -name "bin" -exec rm -rf {} +
	find . -type d -name "obj" -exec rm -rf {} +
	dotnet restore

# Проверка работоспособности проекта
build:
	dotnet build
