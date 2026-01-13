using LlmDashboard.Api.Dtos;
using LlmDashboard.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LlmDashboard.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PromptController : ControllerBase
{
    [HttpGet(Name = "GetPrompts")]
    public IEnumerable<PromptDto> Get()
    {
        return [new PromptDto()
        {
            Id = Guid.NewGuid(),
            Text = "Create C# web api project in .NET 10",
            Status = PromptStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
        }];
    }
}