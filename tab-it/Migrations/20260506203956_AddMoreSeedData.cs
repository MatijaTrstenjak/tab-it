using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace tab_it.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CustomerTabs",
                columns: new[] { "Id", "ClosedAt", "Notes", "OpenedAt", "OpenedByUserId", "Status", "TabCode", "TableNumber" },
                values: new object[,]
                {
                    { 1, null, "", new DateTime(2026, 5, 6, 12, 0, 0, 0, DateTimeKind.Utc), 1, 1, "TAB-001", 5 },
                    { 2, new DateTime(2026, 5, 6, 11, 30, 0, 0, DateTimeKind.Utc), "", new DateTime(2026, 5, 6, 10, 0, 0, 0, DateTimeKind.Utc), 1, 2, "TAB-002", 12 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Drinks and refreshments", "Beverages" },
                    { 2, "Hot meals and main dishes", "Main Course" },
                    { 3, "Sweet treats", "Desserts" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerTabId", "DiscountPercent", "OrderNumber", "OrderedAt", "Status", "Subtotal", "Total" },
                values: new object[,]
                {
                    { 1, 1, 0m, "ORD-001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 15.49m, 15.49m },
                    { 2, 2, 10m, "ORD-002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 18.50m, 16.65m }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AvailableQuantity", "IsAlcoholic", "LastRestockedAt", "Name", "ProductCategoryId", "Sku", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 100, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cola", 1, "BEV-001", 2.50m },
                    { 2, 50, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Coffee", 1, "BEV-002", 3.00m },
                    { 3, 20, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cheeseburger", 2, "MAI-001", 12.99m },
                    { 4, 15, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pepperoni Pizza", 2, "MAI-002", 15.50m },
                    { 5, 10, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chocolate Cake", 3, "DES-001", 6.00m }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "ItemNote", "LineTotal", "OrderId", "ProductId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, "No onions", 12.99m, 1, 3, 1, 12.99m },
                    { 2, "", 2.50m, 1, 1, 1, 2.50m },
                    { 3, "", 15.50m, 2, 4, 1, 15.50m },
                    { 4, "", 3.00m, 2, 2, 1, 3.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CustomerTabs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CustomerTabs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 6, 20, 13, 15, 757, DateTimeKind.Utc).AddTicks(9244));
        }
    }
}
