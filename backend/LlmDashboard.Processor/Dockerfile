FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LlmDashboard.Processor/LlmDashboard.Processor.csproj", "LlmDashboard.Processor/"]
COPY ["LlmDashboard.Domain/LlmDashboard.Domain.csproj", "LlmDashboard.Domain/"]
COPY ["LlmDashboard.Infrastructure/LlmDashboard.Infrastructure.csproj", "LlmDashboard.Infrastructure/"]
RUN dotnet restore "LlmDashboard.Processor/LlmDashboard.Processor.csproj"
COPY . .
WORKDIR "/src/LlmDashboard.Processor"
RUN dotnet build "LlmDashboard.Processor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LlmDashboard.Processor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LlmDashboard.Processor.dll"]
