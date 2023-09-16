using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewsWebApp.Data.Migrations
{
    public partial class AddReviewCreatedTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_CreatorId",
                table: "Reviews");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CreatorId_ReviewItemId",
                table: "Reviews",
                columns: new[] { "CreatorId", "ReviewItemId" },
                unique: true,
                filter: "[CreatorId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_CreatorId_ReviewItemId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CreatorId",
                table: "Reviews",
                column: "CreatorId");
        }
    }
}
