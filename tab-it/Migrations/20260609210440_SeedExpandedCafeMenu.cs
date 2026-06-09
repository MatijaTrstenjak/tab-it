using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace tab_it.Migrations
{
    /// <inheritdoc />
    public partial class SeedExpandedCafeMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Coca Cola 0.33L Can");

            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "Id", "LastRestockedAt", "Name", "QuantityOnHand", "ReorderLevel", "Sku", "Unit" },
                values: new object[,]
                {
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fanta 0.33L Can", 80.000m, 12.000m, "INV-FANTA-033", 1 },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sprite 0.33L Can", 80.000m, 12.000m, "INV-SPRITE-033", 1 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Still Water 0.5L Bottle", 90.000m, 18.000m, "INV-WATER-050", 1 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Orange Juice", 10.000m, 2.000m, "INV-ORANGE-JUICE", 3 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chocolate Cake Slice", 18.000m, 4.000m, "INV-CAKE-CHOC", 1 },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tiramisu Slice", 16.000m, 4.000m, "INV-TIRAMISU", 1 },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cheesecake Slice", 16.000m, 4.000m, "INV-CHEESECAKE", 1 },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Apple Pie Slice", 14.000m, 4.000m, "INV-APPLE-PIE", 1 },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Croissant", 24.000m, 6.000m, "INV-CROISSANT", 1 },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Blueberry Muffin", 20.000m, 5.000m, "INV-MUFFIN-BLUE", 1 },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Brownie", 20.000m, 5.000m, "INV-BROWNIE", 1 },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Burger Patty", 30.000m, 8.000m, "INV-BURGER-PATTY", 1 },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Burger Bun", 30.000m, 8.000m, "INV-BURGER-BUN", 1 },
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pizza Dough", 25.000m, 6.000m, "INV-PIZZA-DOUGH", 1 },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pepperoni", 3.000m, 0.500m, "INV-PEPPERONI", 2 },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cheese", 5.000m, 1.000m, "INV-CHEESE", 2 },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sandwich Bread", 60.000m, 12.000m, "INV-SAND-BREAD", 1 },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chicken", 6.000m, 1.000m, "INV-CHICKEN", 2 },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lettuce", 3.000m, 0.700m, "INV-LETTUCE", 2 },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Caesar Dressing", 4.000m, 0.750m, "INV-CAESAR-DRESS", 3 }
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Coca Cola 0.33L");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Coffee with Milk");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AvailableQuantity", "IsAlcoholic", "LastRestockedAt", "Name", "ProductCategoryId", "Sku", "UnitPrice" },
                values: new object[,]
                {
                    { 6, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Espresso", 1, "BEV-003", 2.20m },
                    { 7, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cappuccino", 1, "BEV-004", 3.40m },
                    { 8, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Latte", 1, "BEV-005", 3.60m },
                    { 9, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fanta 0.33L", 1, "BEV-006", 2.50m },
                    { 10, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sprite 0.33L", 1, "BEV-007", 2.50m },
                    { 11, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Still Water 0.5L", 1, "BEV-008", 1.80m },
                    { 12, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Orange Juice", 1, "BEV-009", 3.20m },
                    { 13, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tiramisu", 3, "DES-002", 5.50m },
                    { 14, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cheesecake", 3, "DES-003", 5.80m },
                    { 15, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Apple Pie", 3, "DES-004", 4.80m },
                    { 16, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Croissant", 3, "DES-005", 2.70m },
                    { 17, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Blueberry Muffin", 3, "DES-006", 3.10m },
                    { 18, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Club Sandwich", 2, "MAI-003", 8.90m },
                    { 19, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Caesar Salad", 2, "MAI-004", 7.80m },
                    { 20, 0, false, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Brownie", 3, "DES-007", 3.90m }
                });

            migrationBuilder.InsertData(
                table: "ProductRecipeItems",
                columns: new[] { "Id", "InventoryItemId", "ProductId", "QuantityRequired" },
                values: new object[,]
                {
                    { 4, 15, 3, 1.000m },
                    { 5, 16, 3, 1.000m },
                    { 6, 17, 4, 1.000m },
                    { 7, 18, 4, 0.050m },
                    { 8, 19, 4, 0.120m },
                    { 9, 8, 5, 1.000m },
                    { 10, 1, 6, 0.009m },
                    { 11, 1, 7, 0.012m },
                    { 12, 2, 7, 0.100m },
                    { 13, 1, 8, 0.012m },
                    { 14, 2, 8, 0.180m },
                    { 15, 4, 9, 1.000m },
                    { 16, 5, 10, 1.000m },
                    { 17, 6, 11, 1.000m },
                    { 18, 7, 12, 0.250m },
                    { 19, 9, 13, 1.000m },
                    { 20, 10, 14, 1.000m },
                    { 21, 11, 15, 1.000m },
                    { 22, 12, 16, 1.000m },
                    { 23, 13, 17, 1.000m },
                    { 24, 20, 18, 2.000m },
                    { 25, 21, 18, 0.120m },
                    { 26, 22, 18, 0.030m },
                    { 27, 21, 19, 0.120m },
                    { 28, 22, 19, 0.120m },
                    { 29, 23, 19, 0.030m },
                    { 30, 14, 20, 1.000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.UpdateData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Cola 0.33L Can");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Cola");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Coffee");
        }
    }
}
