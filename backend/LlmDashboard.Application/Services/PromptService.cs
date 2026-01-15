using LlmDashboard.Api.Dtos;
using LlmDashboard.Application.Repositories;
using LlmDashboard.Domain;
using Microsoft.Extensions.Logging;

namespace LlmDashboard.Application.Services;

public class PromptService : IPromptService
{
    private readonly IPromptRepository _repository;
    private readonly ILogger<PromptService> _logger;

    public PromptService(IPromptRepository repository, ILogger<PromptService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PromptDto> CreateAsync(CreatePromptDto dto, CancellationToken ct = default)
    {
        _logger.LogInformation("Creating new prompt with status: {Status}", dto.Status);

        var prompt = new Prompt
        {
            Id = Guid.NewGuid(),
            Text = dto.Text,
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await _repository.AddAsync(prompt, ct);

        _logger.LogInformation("Successfully created prompt with ID: {PromptId}", prompt.Id);

        return MapToDto(prompt);
    }

    public async Task<PromptDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogDebug("Fetching prompt with ID: {PromptId}", id);

        var prompt = await _repository.GetByIdAsync(id, ct);

        if (prompt is null)
        {
            _logger.LogWarning("Prompt with ID: {PromptId} not found", id);
            return null;
        }

        _logger.LogDebug("Successfully retrieved prompt with ID: {PromptId}", id);

        return MapToDto(prompt);
    }

    public async Task<IEnumerable<PromptDto>> GetManyAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Fetching all prompts");

        var prompts = await _repository.ListAllAsync(ct);

        _logger.LogInformation("Retrieved {Count} prompts", prompts.Count());

        return prompts.Select(MapToDto);
    }

    public async Task<PromptDto?> UpdateAsync(Guid id, UpdatePromptDto dto, CancellationToken ct = default)
    {
        _logger.LogInformation("Updating prompt with ID: {PromptId}", id);

        var prompt = await _repository.GetByIdAsync(id, ct);
        if (prompt is null)
        {
            _logger.LogWarning("Prompt with ID: {PromptId} not found for update", id);
            return null;
        }

        prompt.Text = dto.Text;
        prompt.Status = dto.Status;
        prompt.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(prompt, ct);

        _logger.LogInformation("Successfully updated prompt with ID: {PromptId}", id);

        return MapToDto(prompt);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogInformation("Deleting prompt with ID: {PromptId}", id);

        var prompt = await _repository.GetByIdAsync(id, ct);
        if (prompt is null)
        {
            _logger.LogWarning("Prompt with ID: {PromptId} not found for deletion", id);
            return false;
        }

        await _repository.DeleteAsync(prompt, ct);

        _logger.LogInformation("Successfully deleted prompt with ID: {PromptId}", id);

        return true;
    }

    private static PromptDto MapToDto(Prompt prompt)
        => new PromptDto
        {
            Id = prompt.Id,
            Text = prompt.Text,
            Status = prompt.Status,
            CreatedAt = prompt.CreatedAt,
            UpdatedAt = prompt.UpdatedAt
        };
}