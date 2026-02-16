using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceTicketing.EFDemo.Migrations
{
    /// <inheritdoc />
    public partial class ResolutionSummaryTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResolutionSummary",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResolutionSummary",
                table: "Tickets");
        }
    }
}
