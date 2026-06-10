using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tab_it.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerTabPaymentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "CustomerTabs",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CustomerTabs",
                keyColumn: "Id",
                keyValue: 1,
                column: "PaymentMethod",
                value: "");

            migrationBuilder.UpdateData(
                table: "CustomerTabs",
                keyColumn: "Id",
                keyValue: 2,
                column: "PaymentMethod",
                value: "Cash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "CustomerTabs");
        }
    }
}
