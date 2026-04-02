.PHONY: build run stop restart db-update migrate clean-cache

# Основные команды для запуска
run:
	docker-compose up -d
	dotnet run --project ZooApi.Web/ZooApi.Web.csproj

stop:
	docker-compose down

restart: stop run

# Работа с базой данных (EF Core)
# Пример использования: make migrate NAME=AddOwners
migrate:
	dotnet ef migrations add $(NAME) --project ZooApi.Infrastructure --startup-project ZooApi.Web

db-update:
	dotnet ef database update --project ZooApi.Infrastructure --startup-project ZooApi.Web

# Чистка системы от "призраков" и кэша
clean-cache:
	dotnet clean
	find . -type d -name "bin" -exec rm -rf {} +
	find . -type d -name "obj" -exec rm -rf {} +
	dotnet restore

# Проверка работоспособности проекта
build:
	dotnet build