using LlmDashboard.Domain.Enums;

namespace LlmDashboard.Domain.Models;

public class Prompt
{
    public required Guid Id { get; set; }
    public required string Text { get; set; }
    public required PromptStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public string? OutputText { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}