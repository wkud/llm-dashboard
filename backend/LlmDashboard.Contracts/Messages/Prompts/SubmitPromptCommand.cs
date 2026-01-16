namespace LlmDashboard.Contracts.Messages.Prompts;

public record SubmitPromptCommand
{
    public required Guid PromptId { get; init; }
}
