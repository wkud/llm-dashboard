using LlmDashboard.Application;
using LlmDashboard.Infrastructure;
using LlmDashboard.Processor;
using LlmDashboard.Processor.Clients;
using MassTransit;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((services, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddHttpClient<ILlmClient, OllamaLlmClient>();

// Register MassTransit-based event bus (requires MassTransit to be configured)
builder.Services.AddScoped<LlmDashboard.Application.Abstractions.IEventBus, LlmDashboard.Infrastructure.Messaging.MassTransitEventBus>();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumers(typeof(Program).Assembly);

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
        var rabbitMqUsername = builder.Configuration["RabbitMQ:Username"] ?? "guest";
        var rabbitMqPassword = builder.Configuration["RabbitMQ:Password"] ?? "guest";

        cfg.Host(rabbitMqHost, h =>
        {
            h.Username(rabbitMqUsername);
            h.Password(rabbitMqPassword);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();

try
{
    Log.Information("Starting processor application");
    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}