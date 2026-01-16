# LLM Dashboard

## Prerequisites

- .NET 10.0 SDK
- Docker & Docker Compose
- PostgreSQL (via Docker)
- RabbitMQ (via Docker)

## Getting Started

### 1. Environment Variables Setup

Copy the example environment file and configure your database and RabbitMQ credentials:

```bash
cp .env.example .env
```

Edit `.env` with your preferred credentials. The `.env` file is used by Docker Compose and is excluded from version control.

**Required environment variables:**
- `POSTGRES_DB` - PostgreSQL database name
- `POSTGRES_USER` - PostgreSQL username
- `POSTGRES_PASSWORD` - PostgreSQL password
- `POSTGRES_PORT` - PostgreSQL host port (default: 5433)
- `RABBITMQ_USER` - RabbitMQ username (default: guest)
- `RABBITMQ_PASSWORD` - RabbitMQ password (default: guest)

### 2. .NET User Secrets Setup (Local Development)

For running projects locally outside of Docker, configure .NET user secrets.

**API project:**
```bash
cd backend/LlmDashboard.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5433;Database=llmdashboard;Username=llmuser;Password=change_this_password"
```

**Processor project:**
```bash
cd backend/LlmDashboard.Processor
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5433;Database=llmdashboard;Username=llmuser;Password=change_this_password"
dotnet user-secrets set "RabbitMQ:Username" "guest"
dotnet user-secrets set "RabbitMQ:Password" "guest"
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

Start all services (API + Processor + PostgreSQL + RabbitMQ):

```bash
docker-compose up -d
```

**Services available:**

**API** - REST API service
- HTTP: http://localhost:8080
- HTTPS: http://localhost:8081

**PostgreSQL** - Database
- Host: localhost
- Port: 5433 (configured in .env)

**RabbitMQ** - Message queue
- AMQP Port: 5672
- Management UI: http://localhost:15672
- Default credentials: guest/guest (configurable in .env)

**Processor** - Background worker for processing tasks
- Runs as a background service
- Consumes messages from RabbitMQ

Stop all services:

```bash
docker-compose down
```

To remove volumes (this will delete database and RabbitMQ data):

```bash
docker-compose down -v
```

### 5. Running Locally (Development)

If you want to run the API or Processor locally while using Dockerized services:

1. Start the required services (PostgreSQL and RabbitMQ):
```bash
docker-compose up postgres rabbitmq -d
```

2. Run the API from your IDE or command line:
```bash
cd backend/LlmDashboard.Api
dotnet run
```

3. Or run the Processor:
```bash
cd backend/LlmDashboard.Processor
dotnet run
```

The API will use the connection string from user secrets (pointing to `localhost:5433`).

## Project Structure

```
├── backend/
│   ├── LlmDashboard.Api/          # Web API project
│   ├── LlmDashboard.Processor/    # Background worker
│   ├── LlmDashboard.Application/  # Business logic & services
│   ├── LlmDashboard.Contracts/    # Shared message contracts
│   ├── LlmDashboard.Domain/       # Domain models
│   └── LlmDashboard.Infrastructure/ # Data access & infrastructure
├── .env                            # Environment variables (not in git)
├── .env.example                    # Environment template
└── docker-compose.yml              # Docker services configuration
```

## Architecture

```
                 ┌─────────────┐
                 │   Client    │
                 └──────┬──────┘
                        │ HTTP/S
                        ▼
              ┌──────────────────┐
              │ LlmDashboard.Api │
              │   (Port 8080)    │──────────────────────┐
              └────┬─────────┬───┘                      │
                   │         │                          │
         Reads/    │         │ Publishes                │
         Writes    │         │ Messages                 │
                   ▼         ▼                          │
         ┌──────────────┐  ┌─────────────────────┐      │ References 
         │  PostgreSQL  │  │      RabbitMQ       │      │ shared libs
         │ (Port 5433)  │  │  (Port 5672)        │      │
         └──────┬───────┘  │  UI: localhost:15672│      │
                │          └──────────┬──────────┘      │
                │                     │ Consumes        │
         Reads/ │                     │ Messages        │
         Writes │                     │                 │
                │                     ▼                 │
                │          ┌────────────────────────┐   │
                └──────────│ LlmDashboard.Processor │───│   
                           └────────────────────────┘   │
                                                        │
                           ┌─────────────────────┬──────┴───┬────────────────────┐
                           ▼                     ▼          ▼                    ▼
                  ┌──────────────┐  ┌──────────────┐  ┌──────────────────┐  ┌──────────────┐
                  │  Application │  │  Contracts   │  │  Infrastructure  │  │    Domain    │
                  │(Shared libs) │  │(Shared libs) │  │  (Shared libs)   │  │(Shared libs) │
                  └──────────────┘  └──────────────┘  └──────────────────┘  └──────────────┘
```

**LlmDashboard.Api** - REST API
- CRUD endpoints for prompts
- Publishes messages to RabbitMQ via MassTransit
- Serilog logging to console and file

**LlmDashboard.Processor** - Background Worker
- Consumes messages from RabbitMQ via MassTransit
- Processes long-running tasks asynchronously
- Serilog logging to console and file

**LlmDashboard.Application** - Business Logic
- Service layer for API and Processor operations
- Decoupled business logic from controllers and consumers
- Orchestrates Domain and Infrastructure layers

**LlmDashboard.Contracts** - Message Contracts
- Shared message definitions for MassTransit
- Used by both API and Processor

**LlmDashboard.Domain** - Domain Models
- Domain entities (Prompt, etc.) and enums

**LlmDashboard.Infrastructure** - Data Access
- Entity Framework Core with PostgreSQL
- Database configurations and migrations

## Database Configuration

The project uses Entity Framework Core with PostgreSQL and includes the following features:

- **PostgreSQL with Npgsql provider** - Optimized for PostgreSQL databases
- **Snake case naming convention** - All table and column names use snake_case (e.g., `created_at`, `updated_at`)
- **Automatic retry on failure** - Configured with 3 retry attempts with 30-second max delay
- **Entity configurations** - Fluent API configurations in `Infrastructure/Configurations/` folder
- **Migration assembly** - Migrations are stored in the Infrastructure project

## Logging

Both API and Processor use Serilog for structured logging with daily rolling files and automatic cleanup.

**Log Locations:**
- **API Local**: `backend/LlmDashboard.Api/logs/log-YYYYMMDD.txt`
- **Processor Local**: `backend/LlmDashboard.Processor/logs/log-YYYYMMDD.txt`
- **Docker**: `logs/log-YYYYMMDD.txt` (inside containers at `/app/logs/`)

**Log Levels:**
- Development: Debug (includes EF Core SQL queries)
- Production: Information

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