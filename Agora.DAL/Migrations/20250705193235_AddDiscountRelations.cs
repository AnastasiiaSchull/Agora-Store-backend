using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Subcategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllProducts",
                table: "Discounts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Brands",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_DiscountId",
                table: "Subcategories",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DiscountId",
                table: "Categories",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_DiscountId",
                table: "Brands",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Discounts_DiscountId",
                table: "Brands",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Discounts_DiscountId",
                table: "Categories",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subcategories_Discounts_DiscountId",
                table: "Subcategories",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Discounts_DiscountId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Discounts_DiscountId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Subcategories_Discounts_DiscountId",
                table: "Subcategories");

            migrationBuilder.DropIndex(
                name: "IX_Subcategories_DiscountId",
                table: "Subcategories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_DiscountId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Brands_DiscountId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "AllProducts",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Brands");
        }
    }
}
