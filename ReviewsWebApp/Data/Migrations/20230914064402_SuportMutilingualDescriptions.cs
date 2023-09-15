using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewsWebApp.Data.Migrations
{
    public partial class SuportMutilingualDescriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ReviewsItems",
                newName: "DescriptionRu");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "ReviewsItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "ReviewsItems");

            migrationBuilder.RenameColumn(
                name: "DescriptionRu",
                table: "ReviewsItems",
                newName: "Description");
        }
    }
}
