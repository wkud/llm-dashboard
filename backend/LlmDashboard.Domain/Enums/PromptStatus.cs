namespace LlmDashboard.Domain.Enums;

public enum PromptStatus
{
    None = 0,
    Pending = 1,
    Processing = 2,
    Completed = 3,
    Failed = -1,
}