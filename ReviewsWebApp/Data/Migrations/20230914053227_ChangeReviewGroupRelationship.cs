using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewsWebApp.Data.Migrations
{
    public partial class ChangeReviewGroupRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ReviewsGroup_ReviewGroupId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewGroupId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewGroupId",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "ReviewGroupId",
                table: "ReviewsItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewsItems_ReviewGroupId",
                table: "ReviewsItems",
                column: "ReviewGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewsItems_ReviewsGroup_ReviewGroupId",
                table: "ReviewsItems",
                column: "ReviewGroupId",
                principalTable: "ReviewsGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewsItems_ReviewsGroup_ReviewGroupId",
                table: "ReviewsItems");

            migrationBuilder.DropIndex(
                name: "IX_ReviewsItems_ReviewGroupId",
                table: "ReviewsItems");

            migrationBuilder.DropColumn(
                name: "ReviewGroupId",
                table: "ReviewsItems");

            migrationBuilder.AddColumn<int>(
                name: "ReviewGroupId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewGroupId",
                table: "Reviews",
                column: "ReviewGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ReviewsGroup_ReviewGroupId",
                table: "Reviews",
                column: "ReviewGroupId",
                principalTable: "ReviewsGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
