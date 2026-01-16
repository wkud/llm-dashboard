using System.Net.Http.Json;
using System.Text.Json;
using LlmDashboard.Application.Exceptions;
using LlmDashboard.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace LlmDashboard.Processor.Clients;

public class OllamaLlmClient : ILlmClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly HttpClient _httpClient;
    private readonly ILogger<OllamaLlmClient> _logger;
    private readonly OllamaOptions _options;

    public OllamaLlmClient(
        HttpClient httpClient,
        IOptions<OllamaOptions> options,
        ILogger<OllamaLlmClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<string> ProcessAsync(string prompt, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt);

        _logger.LogDebug(
            "Sending prompt to Ollama using model {Model}",
            _options.Model);

        var request = new OllamaGenerateRequest(
            Model: _options.Model,
            Prompt: prompt,
            Stream: false);

        try
        {
            using var response = await _httpClient.PostAsJsonAsync(
                "api/generate",
                request,
                JsonOptions,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaGenerateResponse>(
                JsonOptions,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(result?.Response))
            {
                throw new LlmClientException("Ollama returned an empty response.");
            }

            _logger.LogDebug("Successfully received response from Ollama");

            return result.Response;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("Request to Ollama was cancelled by caller");
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Request to Ollama timed out");
            throw new LlmClientException("Request to Ollama timed out", ex);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error communicating with Ollama");
            throw new LlmClientException("Failed to communicate with Ollama service", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse Ollama response");
            throw new LlmClientException("Failed to parse Ollama response", ex);
        }
    }
}