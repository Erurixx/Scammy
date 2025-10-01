using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scammy.Migrations
{
    /// <inheritdoc />
    public partial class AddImageS3KeyToArticles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageS3Key",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageS3Key",
                table: "Articles");
        }
    }
}
