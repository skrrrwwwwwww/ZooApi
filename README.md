# 🐾 Zoo Management API

![.NET 9](https://shields.io)
![Architecture](https://shields.io)
![OpenApi](https://shields.io)

> **Enterprise-grade approach to a simple domain.**  
> A robust RESTful API for managing zoo inhabitants and their owners, built with a focus on high-quality architecture and clean code.

---

## 🏗️ Architecture & Engineering

Despite having only two core entities (**Animal** and **Owner**), the project is built using a professional layered architecture comprising **50+ classes**. This ensures strict separation of concerns, high testability, and scalability.

### Project Structure:
*   **Domain**: Core business logic, entities, and state management (Satiety, Happiness).
*   **Application**: Interfaces, DTOs, AutoMapper profiles, and Service implementations.
*   **Infrastructure**: Data persistence and external integrations.
*   **Web API**: Controllers, custom Middleware extensions, and OpenApi configurations.

<!-- Uncomment the line below if you add your dependency graph image to the repo -->
<!-- ![Dependency Graph](./path-to-your-graph-image.png) -->

---

## 🚀 Key Features

### 🦁 Animals Module
*   **Lifecycle Management**: Register new inhabitants, view detailed cards, and handle check-outs.
*   **Interactive Statuses**:
    *   `PUT /feed`: Logic-based feeding system (increases satiety).
    *   `PUT /play`: Activity system (boosts happiness, consumes energy).
*   **Performance**: Built-in pagination for large datasets.

### 👤 Owners Module
*   Complete CRUD for sponsors and animal guardians.
*   **Relational Integrity**: Automated mapping of animals to their respective owners.

---

## 🛠️ Tech Stack

*   **Runtime**: .NET 10 (C# 14)
*   **API Documentation**: `Microsoft.AspNetCore.OpenApi` + Swagger UI.
*   **Object Mapping**: AutoMapper.
*   **Validation**: Data Annotations & Custom Logic.
*   **Patterns**: Dependency Injection, Repository/Service Pattern, Middleware Extensions.

---

## 📖 Getting Started

1. **Clone & Run**:
   ```bash
   git clone https://github.com
   cd ZooApi
   dotnet run --project ZooApi.Web
