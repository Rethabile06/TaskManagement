Solution overview

This document describes the implementation and architecture of the TaskManagement solution. The project follows a Clean Architecture / layered approach to keep concerns separated and make the system easy to test and extend.

Principles

- Clean separation of concerns: Presentation (API) → Application/Services (Core) → Infrastructure (Data).
- Dependency inversion: high-level services depend on abstractions (interfaces) defined in `Core`.
- Single Responsibility & SRP-friendly services: each service implements a focused set of use-cases.

Projects

- `API` (Presentation)
  - ASP.NET minimal/web API controllers expose HTTP endpoints.
  - Composition root (`Program.cs`) configures DI, OpenAPI/Scalar API reference, AutoMapper and the in-memory `TaskDbContext` used for samples and tests.

- `Core` (Domain & Application)
  - Domain entities: `TaskItem`, `TeamMember`, `BaseEntity` and related enums (`TaskStatus`, `TaskPriority`).
  - DTOs / models: `TaskDto`, `TeamMemberDto`, `TaskQuery`.
  - Interfaces: repository and service abstractions (e.g. `ITaskRepository`, `ITaskService`) — these are dependencies for the service layer and decouple business rules from persistence.
  - Services: `TaskService`, `TeamMemberService` implement application use-cases and return a `Result<T>` wrapper for standardized success/error handling.
  - AutoMapper profile centralizes DTO ↔ entity mapping.

- `Infrastructure` (Data)
  - `TaskDbContext` (EF Core) and repository implementations (`TaskRepository`, `TeamMemberRepository`).
  - `DataSeeder` populates sample data on startup.

- `Tests.Unit`
  - Unit tests for service classes (e.g. `TaskServiceTests`, `TeamMemberServiceTests`) that validate business logic in isolation using mocked or in-memory repositories.

Key design patterns and decisions

- Dependency Injection: services and repositories are registered in the API composition root with scoped lifetimes.
- Repository pattern: abstracts data access behind `ITaskRepository` and `ITeamMemberRepository` so services remain persistence-agnostic.
- AutoMapper: reduces manual mapping and keeps DTO/entity mapping in a single place.
- Result wrapper (`Core/Common/Result`) standardizes responses from service methods and avoids throwing exceptions for expected validation or business errors.
- Logging: services use structured logging (`ILogger<T>`) to record important execution events and warnings.
- EF Core InMemory: chosen for simplicity in samples and tests; can be replaced by a relational provider in `Program.cs`.

How responsibilities are split

- Controllers: thin adapters that translate HTTP requests to service calls, return appropriate HTTP status codes.
- Services: implement business rules, validation and orchestrate repository calls.
- Repositories: perform CRUD and query logic against `TaskDbContext`.

Running the solution

- Build: `dotnet restore` && `dotnet build` (solution root).
- Run API: `dotnet run --project API`.
- API docs (local): once running, use the Scalar API Reference at `https://localhost:7033/scalar/v1` to explore endpoints.
- Tests: `dotnet test` from solution root.

Extension and maintenance notes

- Replace persistence: swap the InMemory provider for SQL Server/Postgres by updating `Program.cs` and adding the appropriate EF provider package(s).
- Add validation: integrate FluentValidation or similar in the API layer to centralize request validation.
- Add integration tests: use `WebApplicationFactory<TEntryPoint>` to test controllers and middleware with a test database.
- Add authentication/authorization: wire authentication in `Program.cs` and secure endpoints with policies/attributes.

Important files to review

- `API/Program.cs`
- `API/Controllers/TasksController.cs`
- `Core/Services/TaskService.cs`
- `Core/Services/TeamMemberService.cs`
- `Core/Mappings/MappingProfile.cs`
- `Infrastructure/Data/TaskDbContext.cs`
- `Infrastructure/Data/DataSeeder.cs`

Repository

https://github.com/Rethabile06/TaskManagement
