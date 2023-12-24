using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetroFootballAPI.Migrations
{
    public partial class ModifyVoucherField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoucherID",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Orders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoucherID",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VoucherID",
                table: "Orders",
                column: "VoucherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Voucher_VoucherID",
                table: "Orders",
                column: "VoucherID",
                principalTable: "Voucher",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Voucher_VoucherID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_VoucherID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoucherID",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "Orders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoucherID",
                table: "OrderDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
