using LlmDashboard.Contracts.Messages;
using LlmDashboard.Domain.Enums;
using LlmDashboard.Infrastructure;
using LlmDashboard.Processor.Clients;
using MassTransit;

namespace LlmDashboard.Processor.Consumers;

public class SubmitPromptConsumer : IConsumer<SubmitPromptCommand>
{
    private readonly ApplicationDbContext _db;
    private readonly ILlmClient _llm;
    private readonly ILogger<SubmitPromptConsumer> _logger;

    public SubmitPromptConsumer(ApplicationDbContext db, ILlmClient llm, ILogger<SubmitPromptConsumer> logger)
    {
        _db = db;
        _llm = llm;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SubmitPromptCommand> context)
    {
        var prompt = await _db.Prompts.FindAsync(context.Message.PromptId);

        if (prompt == null)
        {
            _logger.LogWarning("Prompt {PromptId} not found", context.Message.PromptId);
            return;
        }

        // Idempotency guard
        if (prompt.Status != PromptStatus.Pending)
        {
            _logger.LogInformation("Prompt {PromptId} already processed or in progress", prompt.Id);
            return;
        }

        try
        {
            prompt.MarkProcessing();
            await _db.SaveChangesAsync();

            var result = await _llm.ProcessAsync(prompt.InputText);

            prompt.MarkCompleted(result);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed processing prompt {PromptId}", prompt.Id);

            prompt.MarkFailed(ex.Message);
            await _db.SaveChangesAsync();

            throw; // Let MassTransit handle retry / error queue
        }
    }
}