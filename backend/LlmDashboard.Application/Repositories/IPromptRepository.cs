using LlmDashboard.Domain;

namespace LlmDashboard.Application.Repositories;

public interface IPromptRepository
{
    Task<Prompt?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Prompt>> ListAllAsync(CancellationToken ct = default);
    Task AddAsync(Prompt prompt, CancellationToken ct = default);
    Task UpdateAsync(Prompt prompt, CancellationToken ct = default);
    Task DeleteAsync(Prompt prompt, CancellationToken ct = default);
}
