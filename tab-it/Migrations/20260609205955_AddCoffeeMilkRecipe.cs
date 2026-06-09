using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tab_it.Migrations
{
    /// <inheritdoc />
    public partial class AddCoffeeMilkRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductRecipeItems",
                columns: new[] { "Id", "InventoryItemId", "ProductId", "QuantityRequired" },
                values: new object[] { 3, 2, 2, 0.100m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductRecipeItems",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
