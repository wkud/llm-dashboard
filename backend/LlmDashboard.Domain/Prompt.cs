using LlmDashboard.Domain.Enums;

namespace LlmDashboard.Domain;

public class Prompt
{
    public required Guid Id { get; set; }
    public required string Text { get; set; }
    public required PromptStatus Status { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}