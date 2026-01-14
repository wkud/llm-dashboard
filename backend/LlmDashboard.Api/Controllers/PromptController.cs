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

    public PromptController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<PromptDto>> Create([FromBody] CreatePromptDto createDto)
    {
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
        var prompt = await _context.Prompts.FindAsync(id);

        if (prompt is null)
        {
            return NotFound();
        }

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
        var prompts = await _context.Prompts
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

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
        var prompt = await _context.Prompts.FindAsync(id);

        if (prompt is null)
        {
            return NotFound();
        }

        prompt.Text = updateDto.Text;
        prompt.Status = updateDto.Status;
        prompt.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

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
        var prompt = await _context.Prompts.FindAsync(id);

        if (prompt is null)
        {
            return NotFound();
        }

        _context.Prompts.Remove(prompt);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}