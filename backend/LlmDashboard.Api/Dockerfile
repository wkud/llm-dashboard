FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LlmDashboard.Api/LlmDashboard.Api.csproj", "LlmDashboard.Api/"]
COPY ["LlmDashboard.Domain/LlmDashboard.Domain.csproj", "LlmDashboard.Domain/"]
COPY ["LlmDashboard.Infrastructure/LlmDashboard.Infrastructure.csproj", "LlmDashboard.Infrastructure/"]
RUN dotnet restore "LlmDashboard.Api/LlmDashboard.Api.csproj"
COPY . .
WORKDIR "/src/LlmDashboard.Api"
RUN dotnet build "LlmDashboard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LlmDashboard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LlmDashboard.Api.dll"]
