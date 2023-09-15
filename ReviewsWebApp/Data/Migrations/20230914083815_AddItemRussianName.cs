using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewsWebApp.Data.Migrations
{
    public partial class AddItemRussianName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ReviewsItems",
                newName: "NameRu");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "ReviewsItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "ReviewsItems");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "ReviewsItems",
                newName: "Name");
        }
    }
}
