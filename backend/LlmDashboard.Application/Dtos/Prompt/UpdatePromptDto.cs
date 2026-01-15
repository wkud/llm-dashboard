using LlmDashboard.Domain.Enums;

namespace LlmDashboard.Api.Dtos;

public record UpdatePromptDto
{
    public required string Text { get; init; }
    public required PromptStatus Status { get; init; }
}
