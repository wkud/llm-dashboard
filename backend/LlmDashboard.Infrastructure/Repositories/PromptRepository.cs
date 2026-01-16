using LlmDashboard.Application.Abstractions;
using LlmDashboard.Domain;
using LlmDashboard.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LlmDashboard.Infrastructure.Repositories;

public class PromptRepository : IPromptRepository
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<PromptRepository> _logger;

    public PromptRepository(ApplicationDbContext db, ILogger<PromptRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Prompt?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogDebug("Querying database for prompt with ID: {PromptId}", id);

        var prompt = await _db.Prompts.FindAsync(new object[] { id }, ct);

        _logger.LogDebug("Database query completed for prompt ID: {PromptId}, Found: {Found}", id, prompt is not null);
        return prompt;
    }

    public async Task<IEnumerable<Prompt>> ListAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("Querying database for all prompts");

        var prompts = await _db.Prompts
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);

        _logger.LogDebug("Database query completed, retrieved {Count} prompts", prompts.Count);
        return prompts;
    }

    public async Task AddAsync(Prompt prompt, CancellationToken ct = default)
    {
        _logger.LogInformation("Adding prompt to database with ID: {PromptId}", prompt.Id);

        _db.Prompts.Add(prompt);
        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Successfully persisted prompt with ID: {PromptId}", prompt.Id);
    }

    public async Task UpdateAsync(Prompt prompt, CancellationToken ct = default)
    {
        _logger.LogInformation("Updating prompt in database with ID: {PromptId}", prompt.Id);

        _db.Prompts.Update(prompt);
        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Successfully persisted update for prompt with ID: {PromptId}", prompt.Id);
    }

    public async Task DeleteAsync(Prompt prompt, CancellationToken ct = default)
    {
        _logger.LogInformation("Deleting prompt from database with ID: {PromptId}", prompt.Id);

        _db.Prompts.Remove(prompt);
        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Successfully deleted prompt with ID: {PromptId}", prompt.Id);
    }
}