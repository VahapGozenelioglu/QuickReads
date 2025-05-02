using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickReads.Migrations
{
    /// <inheritdoc />
    public partial class ShortcontentaddedtoArticleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortContent",
                table: "Articles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortContent",
                table: "Articles");
        }
    }
}
