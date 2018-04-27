using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAppPhotoSiteImages.Migrations
{
    public partial class Add_Img_and_Comment_entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageCommentMsgs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Comment = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Rate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageCommentMsgs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImagePostMsgs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AverageRate = table.Column<decimal>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    HashTag = table.Column<string>(nullable: true),
                    ImageTitle = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagePostMsgs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageCommentMsgs");

            migrationBuilder.DropTable(
                name: "ImagePostMsgs");
        }
    }
}
