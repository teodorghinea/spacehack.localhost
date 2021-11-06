using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class Competitors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompetitorId",
                table: "FacebookPosts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Competitors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Followers = table.Column<int>(type: "int", nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyOverview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeaturedPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeaturedVideo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneralInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacebookPosts_CompetitorId",
                table: "FacebookPosts",
                column: "CompetitorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacebookPosts_Competitors_CompetitorId",
                table: "FacebookPosts",
                column: "CompetitorId",
                principalTable: "Competitors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacebookPosts_Competitors_CompetitorId",
                table: "FacebookPosts");

            migrationBuilder.DropTable(
                name: "Competitors");

            migrationBuilder.DropIndex(
                name: "IX_FacebookPosts_CompetitorId",
                table: "FacebookPosts");

            migrationBuilder.DropColumn(
                name: "CompetitorId",
                table: "FacebookPosts");
        }
    }
}
