using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAppPhotoSiteImages.Migrations
{
    public partial class Add_new_flds_to_images_and_comments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "ImagePostMsgs",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImagePostMsgId",
                table: "ImageCommentMsgs",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ImageCommentMsgs_ImagePostMsgId",
                table: "ImageCommentMsgs",
                column: "ImagePostMsgId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageCommentMsgs_ImagePostMsgs_ImagePostMsgId",
                table: "ImageCommentMsgs",
                column: "ImagePostMsgId",
                principalTable: "ImagePostMsgs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageCommentMsgs_ImagePostMsgs_ImagePostMsgId",
                table: "ImageCommentMsgs");

            migrationBuilder.DropIndex(
                name: "IX_ImageCommentMsgs_ImagePostMsgId",
                table: "ImageCommentMsgs");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ImagePostMsgs");

            migrationBuilder.DropColumn(
                name: "ImagePostMsgId",
                table: "ImageCommentMsgs");
        }
    }
}
