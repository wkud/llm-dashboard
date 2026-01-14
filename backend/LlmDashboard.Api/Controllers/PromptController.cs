using LlmDashboard.Api.Dtos;
using LlmDashboard.Domain;
using LlmDashboard.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LlmDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromptController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PromptController> _logger;

    public PromptController(ApplicationDbContext context, ILogger<PromptController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<PromptDto>> Create([FromBody] CreatePromptDto createDto)
    {
        _logger.LogInformation("Creating new prompt with status: {Status}", createDto.Status);

        var prompt = new Prompt
        {
            Id = Guid.NewGuid(),
            Text = createDto.Text,
            Status = createDto.Status,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        _context.Prompts.Add(prompt);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Successfully created prompt with ID: {PromptId}", prompt.Id);

        var promptDto = new PromptDto
        {
            Id = prompt.Id,
            Text = prompt.Text,
            Status = prompt.Status,
            CreatedAt = prompt.CreatedAt,
            UpdatedAt = prompt.UpdatedAt
        };

        return CreatedAtAction(nameof(GetById), new { id = prompt.Id }, promptDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PromptDto>> GetById(Guid id)
    {
        _logger.LogDebug("Fetching prompt with ID: {PromptId}", id);

        var prompt = await _context.Prompts.FindAsync(id);

        if (prompt is null)
        {
            _logger.LogWarning("Prompt with ID: {PromptId} not found", id);
            return NotFound();
        }

        _logger.LogDebug("Successfully retrieved prompt with ID: {PromptId}", id);

        var promptDto = new PromptDto
        {
            Id = prompt.Id,
            Text = prompt.Text,
            Status = prompt.Status,
            CreatedAt = prompt.CreatedAt,
            UpdatedAt = prompt.UpdatedAt
        };

        return Ok(promptDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PromptDto>>> GetMany()
    {
        _logger.LogDebug("Fetching all prompts");

        var prompts = await _context.Prompts
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        _logger.LogInformation("Retrieved {Count} prompts", prompts.Count);

        var promptDtos = prompts.Select(p => new PromptDto
        {
            Id = p.Id,
            Text = p.Text,
            Status = p.Status,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });

        return Ok(promptDtos);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PromptDto>> Update(Guid id, [FromBody] UpdatePromptDto updateDto)
    {
        _logger.LogInformation("Updating prompt with ID: {PromptId}", id);

        var prompt = await _context.Prompts.FindAsync(id);

        if (prompt is null)
        {
            _logger.LogWarning("Prompt with ID: {PromptId} not found for update", id);
            return NotFound();
        }

        prompt.Text = updateDto.Text;
        prompt.Status = updateDto.Status;
        prompt.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Successfully updated prompt with ID: {PromptId}", id);

        var promptDto = new PromptDto
        {
            Id = prompt.Id,
            Text = prompt.Text,
            Status = prompt.Status,
            CreatedAt = prompt.CreatedAt,
            UpdatedAt = prompt.UpdatedAt
        };

        return Ok(promptDto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Deleting prompt with ID: {PromptId}", id);

        var prompt = await _context.Prompts.FindAsync(id);

        if (prompt is null)
        {
            _logger.LogWarning("Prompt with ID: {PromptId} not found for deletion", id);
            return NotFound();
        }

        _context.Prompts.Remove(prompt);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Successfully deleted prompt with ID: {PromptId}", id);

        return NoContent();
    }
}