using LlmDashboard.Application.Services;
using LlmDashboard.Contracts.Messages;
using LlmDashboard.Contracts.Messages.Prompts;
using LlmDashboard.Processor.Clients;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace LlmDashboard.Processor.Consumers;

public class SubmitPromptConsumer : IConsumer<SubmitPromptCommand>
{
    private readonly IPromptService _promptService;
    private readonly ILlmClient _llm;
    private readonly ILogger<SubmitPromptConsumer> _logger;

    public SubmitPromptConsumer(
        IPromptService promptService,
        ILlmClient llm,
        ILogger<SubmitPromptConsumer> logger)
    {
        _promptService = promptService;
        _llm = llm;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SubmitPromptCommand> context)
    {
        var promptId = context.Message.PromptId;

        _logger.LogDebug("Processing prompt {PromptId}", promptId);

        var promptDto = await _promptService.GetByIdAsync(promptId);

        if (promptDto == null)
        {
            _logger.LogWarning("Prompt {PromptId} not found", promptId);
            return;
        }

        if (promptDto.Status != Domain.Enums.PromptStatus.Pending)
        {
            _logger.LogInformation("Prompt {PromptId} already processed or in progress", promptId);
            return;
        }

        try
        {
            await _promptService.MarkProcessingAsync(promptId);

            var result = await _llm.ProcessAsync(promptDto.Text);

            await _promptService.MarkCompletedAsync(promptId, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed processing prompt {PromptId}", promptId);

            await _promptService.MarkFailedAsync(promptId, ex.Message);

            throw; // Let MassTransit handle retry / error queue
        }
    }
}