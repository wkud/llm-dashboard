namespace LlmDashboard.Infrastructure;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // public DbSet<Prompt> Prompts { get; set; } = null!;
}