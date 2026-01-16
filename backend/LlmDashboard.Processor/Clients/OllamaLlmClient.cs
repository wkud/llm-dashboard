using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LlmDashboard.Processor.Clients;

public class OllamaLlmClient : ILlmClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OllamaLlmClient> _logger;
    private readonly string _model;

    public OllamaLlmClient(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<OllamaLlmClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _model = configuration["Ollama:Model"] ?? "llama3.2";

        var baseUrl = configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.Timeout = TimeSpan.FromMinutes(5);
    }

    public async Task<string> ProcessAsync(string prompt, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Sending prompt to Ollama using model {Model}", _model);

        var request = new OllamaGenerateRequest
        {
            Model = _model,
            Prompt = prompt,
            Stream = false
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/api/generate",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaGenerateResponse>(
                cancellationToken: cancellationToken);

            if (result?.Response == null)
            {
                throw new InvalidOperationException("Ollama returned empty response");
            }

            _logger.LogDebug("Successfully received response from Ollama");

            return result.Response;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error communicating with Ollama");
            throw new InvalidOperationException("Failed to communicate with Ollama service", ex);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Request to Ollama timed out");
            throw new InvalidOperationException("Request to Ollama timed out", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse Ollama response");
            throw new InvalidOperationException("Failed to parse Ollama response", ex);
        }
    }

    private class OllamaGenerateRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = string.Empty;

        [JsonPropertyName("stream")]
        public bool Stream { get; set; }
    }

    private class OllamaGenerateResponse
    {
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("response")]
        public string? Response { get; set; }

        [JsonPropertyName("done")]
        public bool Done { get; set; }
    }
}
