using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetroFootballAPI.Migrations
{
    public partial class ModifyFieldVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_Customers_CustomerID",
                table: "Voucher");

            migrationBuilder.DropIndex(
                name: "IX_Voucher_CustomerID",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Voucher");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "VoucherApplied",
                newName: "CustomerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "VoucherApplied",
                newName: "ProductID");

            migrationBuilder.AddColumn<string>(
                name: "CustomerID",
                table: "Voucher",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_CustomerID",
                table: "Voucher",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_Customers_CustomerID",
                table: "Voucher",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "ID");
        }
    }
}
