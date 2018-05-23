using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAppPhotoSiteImages.Migrations
{
    public partial class add_rating_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "ImageCommentMsgs");

            migrationBuilder.CreateTable(
                name: "ImageRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ImageId = table.Column<Guid>(nullable: false),
                    Rate = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageRatings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageRatings");

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "ImageCommentMsgs",
                nullable: false,
                defaultValue: 0);
        }
    }
}
