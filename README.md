🐾 Zoo Management API


A sophisticated RESTful API designed for managing zoo inhabitants and their owners. While the domain focuses on two primary entities, the project serves as a showcase for enterprise-grade architecture, scalability, and clean code principles.
🏗️ Architecture & Engineering
The project comprises 50+ classes, organized into a strictly decoupled layered architecture. This ensures high testability, maintainability, and a clear separation of concerns.
Domain Layer: Contains core entities (Animal, Owner) and business logic.
Application Layer: Handles orchestration via services, DTO mapping (AutoMapper), and business rules.
Infrastructure & Web: Implements data persistence, API controllers, and custom middleware extensions for a seamless developer experience.
🚀 Key Features
🦁 Animal Management
Full Lifecycle: Registration, detailed profiles, and checkout (deletion) of zoo residents.
Interactive Endpoints:
PUT /feed: Increase satiety levels (logic-driven status updates).
PUT /play: Boost happiness while managing energy depletion.
Optimized Queries: Built-in pagination and filtering for performance.
👤 Owner & Sponsor Tracking
Manage a database of sponsors and legal owners.
Relational Logic: Established One-to-Many relationships, ensuring every animal is correctly linked to its human guardian.
🛠️ Tech Stack
Runtime: .NET 9 (C# 13)
API Documentation: Native Microsoft.AspNetCore.OpenApi with Swagger UI integration.
Object Mapping: AutoMapper for clean DTO/Entity separation.
Validation: Robust validation logic using Data Annotations and Fluent patterns.
Patterns: Dependency Injection (DI), Repository/Service patterns, and Middleware Extensions.
📖 Getting Started
Clone the repository:
bash
git clone https://github.com
Используйте код с осторожностью.

Run the application:
bash
dotnet run --project ZooApi.Web
Используйте код с осторожностью.

Explore the API:
Navigate to http://localhost:5000/ or /swagger to access the interactive Swagger UI.
