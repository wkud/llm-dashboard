namespace LlmDashboard.Application.Dtos.Prompt;

public record CreatePromptDto
{
    public required string Text { get; init; }
}
