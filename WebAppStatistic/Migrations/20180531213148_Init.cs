using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebAppStatistic.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Action = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: true),
                    UserInfo = table.Column<string>(nullable: false),
                    UserProfileId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserActions");
        }
    }
}
