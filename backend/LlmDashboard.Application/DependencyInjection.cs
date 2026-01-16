using LlmDashboard.Application.Abstractions;
using LlmDashboard.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LlmDashboard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPromptService, PromptService>();

        return services;
    }
}
