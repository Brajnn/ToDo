# ToDo REST API

A clean, scalable REST API for managing personal tasks, built with .NET 8 and Clean Architecture principles. Features include:

- CRUD operations for tasks
- Setting task completion percentages
- Marking tasks as done
- Retrieving tasks scheduled for today, tomorrow, or the current week

Containerized deployment using Docker and Docker Compose
Key goals of the project:
- Build a scalable and maintainable REST API
- Apply CQRS patterns using MediatR
- Integrate a real database (MySQL) with Entity Framework Core
- Ensure high-quality code with full unit test coverage
- Evaluate API performance through benchmarking
- Enable simple and portable deployment with Docker and Docker Compose
  
 ## Technologies

- **.NET 8** – Core framework used for building the API.
- **ASP.NET Core Web API** – For creating RESTful endpoints.
- **Entity Framework Core (Pomelo Provider for MySQL)** – ORM for database operations.
- **MySQL** – Relational database used to persist ToDo tasks.
- **MediatR** – Implements CQRS (Command Query Responsibility Segregation) and decouples business logic.
- **xUnit, Moq, FluentAssertions** – Tools for unit and integration testing.
- **BenchmarkDotNet** – Framework for measuring API performance.
- **Docker & Docker Compose** – Used for containerization and orchestrating services.
- **Clean Architecture principles** – To separate concerns across layers (Domain, Application, Infrastructure, Presentation).
- 
  ## Key Features

- **Create Todo** – Add a new task with a title, description, expiry date, and default completion status.
- **Get All Todos** – Retrieve a list of all existing tasks.
- **Get Todo by ID** – Fetch detailed information about a specific task using its unique identifier.
- **Get Incoming Todos** – Retrieve tasks that are due today, tomorrow, or during the current week.
- **Update Todo** – Modify the title, description, expiry date, or other task details.
- **Set Todo Percent Complete** – Update the percentage of completion for a task.
- **Mark Todo as Done** – Mark a task as fully completed.
- **Delete Todo** – Remove a task permanently from the database.
- **Validation** – All input data is validated before being processed.
- **Unit Testing** – Full unit test coverage for commands, queries, and repository logic.
- **Performance Testing** – Benchmarking of critical API endpoints using BenchmarkDotNet.
- **Containerization** – Deployment-ready via Docker and Docker Compose.

## Architecture

The project follows the **Clean Architecture** pattern, which separates concerns across different layers and enforces strong boundaries between them:

- **Domain** – Contains core entities and interfaces that define the business logic.
- **Application** – Contains use cases (commands and queries) and DTOs. Implements business rules without any dependency on infrastructure.
- **Infrastructure** – Implements database persistence, repositories, and external service access.
- **WebAPI (Presentation)** – Exposes RESTful endpoints and handles HTTP requests and responses.

Key architectural principles used:
- **Dependency Inversion** – Higher-level modules (Application) do not depend on lower-level modules (Infrastructure).
- **CQRS (Command Query Responsibility Segregation)** – Commands (mutations) and Queries (reads) are handled separately using **MediatR**.
- **Dependency Injection** – All services are injected via built-in .NET Core DI container.
- **Separation of Concerns** – Clear division between application logic, domain logic, infrastructure access, and presentation layer.

## Setup and Installation

To run the project locally using Docker, follow these steps:

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (includes Docker Compose)
  
### Cloning the Repository

First, clone the repository to your local machine:

```
git clone https://github.com/Brajnn/ToDo.git
```
Navigate into the project directory:
```
cd ToDo
```
### Environment Variables

Create a `.env` file in the root of the project directory with the following keys:
```
MYSQL_ROOT_PASSWORD=
MYSQL_DATABASE=
MYSQL_USER=
MYSQL_PASSWORD=
```
You must fill in these values with your own database credentials.  
They will be used to configure the MySQL database inside Docker.
  
These variables are used to configure the MySQL database inside Docker.

### Running the application

1. Open a terminal and navigate to the project directory.

2. Run the following command to build and start the containers:
```
docker-compose up --build
```
3.The API will be available at:
```
http://localhost:8080/api/todo
```
4. The MySQL database will be accessible on port 3306 inside the tododb container.

## Usage (API Endpoints)

All API endpoints are prefixed with `/api/todo`.

Base URL when running locally:
```
http://localhost:8080/api/todo
```

### Endpoints

| Method | Endpoint                          | Description                                  |
|--------|-----------------------------------|----------------------------------------------|
| GET    | `/api/todo`                       | Get all todos                                |
| GET    | `/api/todo/{id}`                  | Get a specific todo by ID                    |
| GET    | `/api/todo/incoming?range={value}` | Get incoming todos (today, tomorrow, week)  |
| POST   | `/api/todo`                       | Create a new todo                            |
| PUT    | `/api/todo/{id}`                  | Update an existing todo                      |
| PATCH  | `/api/todo/{id}/percent`           | Set percent complete for a todo             |
| PATCH  | `/api/todo/{id}/done`              | Mark a todo as done                         |
| DELETE | `/api/todo/{id}`                  | Delete a todo                                |

---
Example request:
```
POST http://localhost:8080/api/todo
```
Example Request Body (JSON):
```
{
  "title": "Example",
  "description": "Test description",
  "expiryDate": "2025-04-23T17:00:00"
}
```
## Testing

The project includes:

- **Unit Tests** – covering commands, queries, and repositories.
- **Performance Tests** – benchmarking API operations.

### Running Unit Tests

```
dotnet test
```
Running Performance Tests
```
cd ToDo.PerformanceTests
dotnet run -c Release
```
Make sure to run performance tests in Release configuration.

## Performance Results
Performance benchmarks were executed using BenchmarkDotNet.

| Method           | Mean (ms) |
|------------------|-----------|
| GetAllTodos      | 23.29     |
| GetTodoById      | 1.97      |
| GetIncomingTodos | 5.73      | 
| CreateTodo       | 4.97      |
| UpdateTodo       | 6.58      | 
| SetTodoPercent   | 2.94      | 
| MarkTodoAsDone   | 3.08      |
| DeleteTodo       | 12.6      | 

## Contact
**Email**: brajanalterman@gmail.com

