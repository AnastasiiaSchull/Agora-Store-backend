using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteForFAQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FAQs_FAQCategories_FAQCategoryId",
                table: "FAQs");

            migrationBuilder.AddForeignKey(
                name: "FK_FAQs_FAQCategories_FAQCategoryId",
                table: "FAQs",
                column: "FAQCategoryId",
                principalTable: "FAQCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FAQs_FAQCategories_FAQCategoryId",
                table: "FAQs");

            migrationBuilder.AddForeignKey(
                name: "FK_FAQs_FAQCategories_FAQCategoryId",
                table: "FAQs",
                column: "FAQCategoryId",
                principalTable: "FAQCategories",
                principalColumn: "Id");
        }
    }
}
