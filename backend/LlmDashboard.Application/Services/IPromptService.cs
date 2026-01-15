using LlmDashboard.Api.Dtos;

namespace LlmDashboard.Application.Services;

public interface IPromptService
{
    Task<PromptDto> CreateAsync(CreatePromptDto dto, CancellationToken ct = default);
    Task<PromptDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<PromptDto>> GetManyAsync(CancellationToken ct = default);
    Task<PromptDto?> UpdateAsync(Guid id, UpdatePromptDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}