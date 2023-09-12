using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewsWebApp.Data.Migrations
{
    public partial class RemoveLocaleFromGroupModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ReviewsGroup",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ReviewsGroup",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ReviewsGroup",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ReviewsGroup",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ReviewsGroup",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ReviewsGroup",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "Locale",
                table: "ReviewsGroup");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ReviewsGroup",
                newName: "NameRu");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewsGroup_Name",
                table: "ReviewsGroup",
                newName: "IX_ReviewsGroup_NameRu");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "ReviewsGroup",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "ReviewsGroup",
                columns: new[] { "Id", "NameEn", "NameRu" },
                values: new object[] { 100, "Movies", "Кино" });

            migrationBuilder.InsertData(
                table: "ReviewsGroup",
                columns: new[] { "Id", "NameEn", "NameRu" },
                values: new object[] { 101, "Books", "Книги" });

            migrationBuilder.InsertData(
                table: "ReviewsGroup",
                columns: new[] { "Id", "NameEn", "NameRu" },
                values: new object[] { 201, "Games", "Игры" });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewsGroup_NameEn",
                table: "ReviewsGroup",
                column: "NameEn",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReviewsGroup_NameEn",
                table: "ReviewsGroup");

            migrationBuilder.DeleteData(
                table: "ReviewsGroup",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "ReviewsGroup",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "ReviewsGroup",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "ReviewsGroup");

            migrationBuilder.RenameColumn(
                name: "NameRu",
                table: "ReviewsGroup",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewsGroup_NameRu",
                table: "ReviewsGroup",
                newName: "IX_ReviewsGroup_Name");

            migrationBuilder.AddColumn<int>(
                name: "Locale",
                table: "ReviewsGroup",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "ReviewsGroup",
                columns: new[] { "Id", "Locale", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Movies" },
                    { 2, 1, "Кино" },
                    { 3, 0, "Books" },
                    { 4, 1, "Книги" },
                    { 5, 0, "Games" },
                    { 6, 1, "Игры" }
                });
        }
    }
}
