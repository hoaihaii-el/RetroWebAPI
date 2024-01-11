using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetroFootballAPI.Migrations
{
    public partial class UpdateXoaVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Xoa",
                table: "Voucher",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Xoa",
                table: "Voucher");
        }
    }
}
