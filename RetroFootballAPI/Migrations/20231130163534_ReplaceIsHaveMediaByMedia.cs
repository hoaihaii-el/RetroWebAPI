using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetroFootballAPI.Migrations
{
    public partial class ReplaceIsHaveMediaByMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHaveMedia",
                table: "Feedbacks");

            migrationBuilder.AddColumn<string>(
                name: "Media",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Media",
                table: "Feedbacks");

            migrationBuilder.AddColumn<bool>(
                name: "IsHaveMedia",
                table: "Feedbacks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
