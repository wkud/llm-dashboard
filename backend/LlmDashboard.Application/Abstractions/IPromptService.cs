using LlmDashboard.Application.Dtos.Prompt;

namespace LlmDashboard.Application.Abstractions;

public interface IPromptService
{
    Task<PromptDto> CreateAsync(CreatePromptDto dto, CancellationToken ct = default);
    Task<PromptDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<PromptDto>> GetManyAsync(CancellationToken ct = default);
    Task<PromptDto?> UpdateAsync(Guid id, UpdatePromptDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);

    Task MarkProcessingAsync(Guid id, CancellationToken ct = default);
    Task MarkCompletedAsync(Guid id, string output, CancellationToken ct = default);
    Task MarkFailedAsync(Guid id, string error, CancellationToken ct = default);
}