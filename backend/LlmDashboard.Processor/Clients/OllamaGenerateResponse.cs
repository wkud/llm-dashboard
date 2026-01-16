using System.Text.Json.Serialization;

namespace LlmDashboard.Processor.Clients;

internal record OllamaGenerateResponse(
    [property: JsonPropertyName("model")] string? Model,
    [property: JsonPropertyName("response")] string? Response,
    [property: JsonPropertyName("done")] bool Done
);