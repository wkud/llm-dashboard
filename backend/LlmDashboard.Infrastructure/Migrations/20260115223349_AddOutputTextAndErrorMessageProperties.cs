using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LlmDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOutputTextAndErrorMessageProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "error_message",
                table: "prompts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "output_text",
                table: "prompts",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "error_message",
                table: "prompts");

            migrationBuilder.DropColumn(
                name: "output_text",
                table: "prompts");
        }
    }
}
