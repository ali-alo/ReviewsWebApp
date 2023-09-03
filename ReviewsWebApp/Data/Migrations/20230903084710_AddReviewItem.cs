using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewsWebApp.Data.Migrations
{
    public partial class AddReviewItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewFor",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "ReviewItemId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ReviewItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageLink = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewItem", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewItemId",
                table: "Reviews",
                column: "ReviewItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ReviewItem_ReviewItemId",
                table: "Reviews",
                column: "ReviewItemId",
                principalTable: "ReviewItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ReviewItem_ReviewItemId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "ReviewItem");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewItemId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewItemId",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "ReviewFor",
                table: "Reviews",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
