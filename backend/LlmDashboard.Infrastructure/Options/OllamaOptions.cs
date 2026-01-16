namespace LlmDashboard.Infrastructure.Options;

public class OllamaOptions
{
    public const string SectionName = "Ollama";
    
    public string BaseUrl { get; init; } = "http://localhost:11434";
    public string Model { get; init; } = "llama3.2";
    public TimeSpan Timeout { get; init; } = TimeSpan.FromMinutes(5);
}