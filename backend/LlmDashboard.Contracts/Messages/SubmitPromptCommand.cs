namespace LlmDashboard.Contracts.Messages;

public record SubmitPromptCommand
{
    public required Guid PromptId { get; init; }
}
