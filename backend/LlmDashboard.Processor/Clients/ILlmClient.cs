namespace LlmDashboard.Processor.Clients;

public interface ILlmClient
{
    Task<string> ProcessAsync(string prompt, CancellationToken cancellationToken = default);
}
