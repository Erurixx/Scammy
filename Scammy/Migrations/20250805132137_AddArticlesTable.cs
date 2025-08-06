using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scammy.Migrations
{
    /// <inheritdoc />
    public partial class AddArticlesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Articles",
                newName: "DatePosted");

            migrationBuilder.AddColumn<string>(
                name: "AdminComment",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminComment",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "DatePosted",
                table: "Articles",
                newName: "DateCreated");
        }
    }
}
