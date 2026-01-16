using LlmDashboard.Application.Abstractions;
using LlmDashboard.Infrastructure.Messaging;
using LlmDashboard.Infrastructure.Options;
using LlmDashboard.Infrastructure.Repositories;
using Microsoft.Extensions.Options;

namespace LlmDashboard.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var databaseOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;

            options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: databaseOptions.MaxRetryCount,
                        maxRetryDelay: databaseOptions.MaxRetryDelay,
                        errorCodesToAdd: null);
                })
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IPromptRepository, PromptRepository>();
        services.AddScoped<IEventBus, MassTransitEventBus>();

        return services;
    }
}