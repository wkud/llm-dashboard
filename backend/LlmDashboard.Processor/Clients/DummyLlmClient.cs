namespace LlmDashboard.Processor.Clients;

public class DummyLlmClient : ILlmClient
{
    public async Task<string> ProcessAsync(string prompt, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult("dummy result");
    }
}