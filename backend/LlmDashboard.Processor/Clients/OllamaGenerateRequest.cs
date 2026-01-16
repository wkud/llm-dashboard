using System.Text.Json.Serialization;

namespace LlmDashboard.Processor.Clients;

internal record OllamaGenerateRequest(
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("prompt")] string Prompt,
    [property: JsonPropertyName("stream")] bool Stream
);