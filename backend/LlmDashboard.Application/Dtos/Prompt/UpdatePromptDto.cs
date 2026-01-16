using LlmDashboard.Domain.Enums;

namespace LlmDashboard.Application.Dtos.Prompt;

public record UpdatePromptDto
{
    public required string Text { get; init; }
    public required PromptStatus Status { get; init; }
}
