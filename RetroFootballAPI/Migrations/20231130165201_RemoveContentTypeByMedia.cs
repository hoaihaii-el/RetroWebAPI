using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetroFootballAPI.Migrations
{
    public partial class RemoveContentTypeByMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Media",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Media",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "ContentType",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
