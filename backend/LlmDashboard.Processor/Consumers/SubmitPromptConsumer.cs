using LlmDashboard.Contracts.Messages;
using LlmDashboard.Infrastructure;
using MassTransit;

namespace LlmDashboard.Processor.Consumers;

public class SubmitPromptConsumer(
    ILogger<SubmitPromptConsumer> logger,
    ApplicationDbContext dbContext) : IConsumer<SubmitPromptCommand>
{
    public async Task Consume(ConsumeContext<SubmitPromptCommand> context)
    {
        var promptId = context.Message.PromptId;

        logger.LogInformation("Processing prompt with ID: {PromptId}", promptId);

        var prompt = await dbContext.Prompts.FindAsync(promptId);

        if (prompt == null)
        {
            logger.LogWarning("Prompt with ID {PromptId} not found", promptId);
            return;
        }

        logger.LogInformation("Found prompt with ID: {PromptId}", prompt.Id);

        // TODO: Add your prompt processing logic here
        // For now, just simulate some work
        await Task.Delay(1000);

        logger.LogInformation("Completed processing prompt with ID: {PromptId}", promptId);
    }
}
