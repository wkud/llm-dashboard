namespace LlmDashboard.Infrastructure.Options;
    
public class DatabaseOptions
{
    public const string SectionName = "Database";
    
    public int MaxRetryCount { get; init; } = 3;
    public TimeSpan MaxRetryDelay { get; init; } = TimeSpan.FromSeconds(30);
}