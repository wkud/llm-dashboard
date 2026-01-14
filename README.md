# LLM Dashboard

## Prerequisites

- .NET 8.0 SDK
- Docker & Docker Compose
- PostgreSQL (via Docker)

## Getting Started

### 1. Environment Variables Setup

Copy the example environment file and configure your database credentials:

```bash
cp .env.example .env
```

Edit `.env` with your preferred database credentials. The `.env` file is used by Docker Compose and is excluded from version control.

### 2. .NET User Secrets Setup (Local Development)

For running the API locally outside of Docker, you need to configure .NET user secrets with the database connection string.

Navigate to the API project directory:

```bash
cd backend/LlmDashboard.Api
```

Initialize user secrets for the project:

```bash
dotnet user-secrets init
```

Set the database connection string (update values to match your `.env` file):

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=llmdashboard;Username=llmuser;Password=change_this_password"
```

**Important Notes:**
- User secrets are stored outside the project directory and are never committed to source control
- Use `Host=localhost` for local development (connecting to Docker postgres from your host machine)
- Use `Host=postgres` in Docker Compose environment (container-to-container communication)

To view your configured secrets:

```bash
dotnet user-secrets list
```

To remove a secret:

```bash
dotnet user-secrets remove "ConnectionStrings:DefaultConnection"
```

To clear all secrets:

```bash
dotnet user-secrets clear
```

### 3. Database Migrations

The project uses Entity Framework Core for database migrations. Before running the application, you need to create and apply migrations.

Navigate to the Infrastructure project directory:

```bash
cd backend/LlmDashboard.Infrastructure
```

Create a new migration:

```bash
dotnet ef migrations add InitialCreate --startup-project ../LlmDashboard.Api
```

Apply migrations to the database:

```bash
dotnet ef database update --startup-project ../LlmDashboard.Api
```

**Important Notes:**
- Make sure PostgreSQL is running before applying migrations (either via Docker or locally)
- The `--startup-project` flag is required because migrations are in Infrastructure but the connection string is configured in the API project
- For local development, ensure your user secrets are configured before running migrations

Common migration commands:

List all migrations:
```bash
dotnet ef migrations list --startup-project ../LlmDashboard.Api
```

Remove the last migration (before applying it):
```bash
dotnet ef migrations remove --startup-project ../LlmDashboard.Api
```

Generate SQL script for a migration:
```bash
dotnet ef migrations script --startup-project ../LlmDashboard.Api
```

### 4. Running with Docker Compose

Start all services (API + PostgreSQL):

```bash
docker-compose up -d
```

The API will be available at:
- HTTP: http://localhost:8080
- HTTPS: http://localhost:8081

PostgreSQL will be available at:
- Host: localhost
- Port: 5432

Stop all services:

```bash
docker-compose down
```

To remove volumes (this will delete database data):

```bash
docker-compose down -v
```

### 5. Running Locally (Development)

If you want to run the API locally while using the Dockerized PostgreSQL:

1. Start only the PostgreSQL service:
```bash
docker-compose up postgres -d
```

2. Run the API from your IDE or command line:
```bash
cd backend/LlmDashboard.Api
dotnet run
```

The API will use the connection string from user secrets (pointing to `localhost:5432`).

## Project Structure

```
├── backend/
│   ├── LlmDashboard.Api/          # Web API project
│   ├── LlmDashboard.Domain/       # Domain models
│   └── LlmDashboard.Infrastructure/ # Data access & infrastructure
├── .env                            # Environment variables (not in git)
├── .env.example                    # Environment template
└── docker-compose.yml              # Docker services configuration
```

## Database Configuration

The project uses Entity Framework Core with PostgreSQL and includes the following features:

- **PostgreSQL with Npgsql provider** - Optimized for PostgreSQL databases
- **Snake case naming convention** - All table and column names use snake_case (e.g., `created_at`, `updated_at`)
- **Automatic retry on failure** - Configured with 3 retry attempts with 30-second max delay
- **Entity configurations** - Fluent API configurations in `Infrastructure/Configurations/` folder
- **Migration assembly** - Migrations are stored in the Infrastructure project

## Logging

The project uses Serilog for structured logging. All log files are written with daily rolling and automatic cleanup.

**Log Locations:**
- **Local Development**: `backend/LlmDashboard.Api/logs/log-YYYYMMDD.txt`
- **Docker/Production**: `logs/log-YYYYMMDD.txt` (inside the container at `/app/logs/`)

**Log Levels:**
- Development: Debug level (includes EF Core SQL queries)
- Production: Information level

**Development Guidelines:**
When writing new code, use `ILogger<T>` for logging. Inject the logger via constructor and use structured logging with named parameters:

```csharp
public class MyController : ControllerBase
{
    private readonly ILogger<MyController> _logger;

    public MyController(ILogger<MyController> logger)
    {
        _logger = logger;
    }

    public void MyMethod(Guid id)
    {
        _logger.LogInformation("Processing item with ID: {ItemId}", id);
        
        // process the item
        
        _logger.LogInformation("Processed item with ID: {ItemId}", id);
    }
}
```