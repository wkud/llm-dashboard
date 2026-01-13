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

### 3. Running with Docker Compose

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

### 4. Running Locally (Development)

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