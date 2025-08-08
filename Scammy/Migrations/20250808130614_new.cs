using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scammy.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DatePosted",
                table: "Articles",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Excerpt",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Excerpt",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Articles",
                newName: "DatePosted");
        }
    }
}
