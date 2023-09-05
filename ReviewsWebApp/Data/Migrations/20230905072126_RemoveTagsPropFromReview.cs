using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewsWebApp.Data.Migrations
{
    public partial class RemoveTagsPropFromReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ReviewItem_ReviewItemId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Reviews_ReviewId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ReviewId",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewItem",
                table: "ReviewItem");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Tags");

            migrationBuilder.RenameTable(
                name: "ReviewItem",
                newName: "ReviewsItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewsItems",
                table: "ReviewsItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ReviewsItems_ReviewItemId",
                table: "Reviews",
                column: "ReviewItemId",
                principalTable: "ReviewsItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ReviewsItems_ReviewItemId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewsItems",
                table: "ReviewsItems");

            migrationBuilder.RenameTable(
                name: "ReviewsItems",
                newName: "ReviewItem");

            migrationBuilder.AddColumn<int>(
                name: "ReviewId",
                table: "Tags",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewItem",
                table: "ReviewItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ReviewId",
                table: "Tags",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ReviewItem_ReviewItemId",
                table: "Reviews",
                column: "ReviewItemId",
                principalTable: "ReviewItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Reviews_ReviewId",
                table: "Tags",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }
    }
}
