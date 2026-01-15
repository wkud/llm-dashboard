using LlmDashboard.Api.Dtos;
using LlmDashboard.Application.Dtos.Prompt;
using LlmDashboard.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LlmDashboard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromptController : ControllerBase
{
    private readonly IPromptService _service;
    private readonly ILogger<PromptController> _logger;

    public PromptController(IPromptService service, ILogger<PromptController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<PromptDto>> Create([FromBody] CreatePromptDto dto)
    {
        var promptDto = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = promptDto.Id }, promptDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PromptDto>> GetById(Guid id)
    {
        var promptDto = await _service.GetByIdAsync(id);
        return promptDto is null ? NotFound() : Ok(promptDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PromptDto>>> GetMany()
        => Ok(await _service.GetManyAsync());

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PromptDto>> Update(Guid id, [FromBody] UpdatePromptDto dto)
    {
        var promptDto = await _service.UpdateAsync(id, dto);
        return promptDto is null ? NotFound() : Ok(promptDto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}