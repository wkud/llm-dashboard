using LlmDashboard.Application.Abstractions;
using LlmDashboard.Application.Dtos.Prompt;
using LlmDashboard.Application.Exceptions;
using LlmDashboard.Contracts.Messages.Prompts;
using LlmDashboard.Domain.Enums;
using LlmDashboard.Domain.Models;
using Microsoft.Extensions.Logging;

namespace LlmDashboard.Application.Services;

public class PromptService : IPromptService
{
    private readonly IPromptRepository _repository;
    private readonly ILogger<PromptService> _logger;
    private readonly IEventBus _eventBus;

    public PromptService(IPromptRepository repository, ILogger<PromptService> logger, IEventBus eventBus)
    {
        _repository = repository;
        _logger = logger;
        _eventBus = eventBus;
    }

    public async Task<PromptDto> CreateAsync(CreatePromptDto dto, CancellationToken ct = default)
    {
        _logger.LogInformation("Creating new prompt");

        var prompt = new Prompt
        {
            Id = Guid.NewGuid(),
            Text = dto.Text,
            Status = PromptStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(prompt, ct);

        await _eventBus.PublishAsync(new SubmitPromptCommand { PromptId = prompt.Id }, ct);
        _logger.LogDebug("Successfully created new prompt with ID: {PromptId}", prompt.Id);

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
    
    public async Task MarkProcessingAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogInformation("Marking prompt {PromptId} as processing", id);

        var prompt = await _repository.GetByIdAsync(id, ct);
        if (prompt == null)
        {
            _logger.LogWarning("Prompt {PromptId} not found for marking as processing", id);
            throw new NotFoundException($"Prompt {id} not found");
        }

        if (prompt.Status != PromptStatus.Pending)
        {
            _logger.LogWarning("Cannot mark prompt {PromptId} as processing - current status is {Status}", id, prompt.Status);
            throw new InvalidOperationException("Prompt must be pending to start processing");
        }

        prompt.Status = PromptStatus.Processing;
        prompt.UpdatedAt = DateTime.UtcNow;
        prompt.ErrorMessage = null;

        await _repository.UpdateAsync(prompt, ct);

        _logger.LogInformation("Successfully marked prompt {PromptId} as processing", id);
    }

    public async Task MarkCompletedAsync(Guid id, string output, CancellationToken ct = default)
    {
        _logger.LogInformation("Marking prompt {PromptId} as completed", id);

        var prompt = await _repository.GetByIdAsync(id, ct);
        if (prompt == null)
        {
            _logger.LogWarning("Prompt {PromptId} not found for marking as completed", id);
            throw new NotFoundException($"Prompt {id} not found");
        }

        prompt.Status = PromptStatus.Completed;
        prompt.OutputText = output;
        prompt.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(prompt, ct);

        _logger.LogInformation("Successfully marked prompt {PromptId} as completed", id);
    }

    public async Task MarkFailedAsync(Guid id, string error, CancellationToken ct = default)
    {
        _logger.LogWarning("Marking prompt {PromptId} as failed with error: {Error}", id, error);

        var prompt = await _repository.GetByIdAsync(id, ct);
        if (prompt == null)
        {
            _logger.LogWarning("Prompt {PromptId} not found for marking as failed", id);
            throw new NotFoundException($"Prompt {id} not found");
        }

        prompt.Status = PromptStatus.Failed;
        prompt.ErrorMessage = error;
        prompt.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(prompt, ct);

        _logger.LogWarning("Successfully marked prompt {PromptId} as failed", id);
    }

    private static PromptDto MapToDto(Prompt prompt)
        => new PromptDto
        {
            Id = prompt.Id,
            Text = prompt.Text,
            Status = prompt.Status,
            ErrorMessage = prompt.ErrorMessage,
            OutputText = prompt.OutputText,
            CreatedAt = prompt.CreatedAt,
            UpdatedAt = prompt.UpdatedAt
        };
}