using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addBrandSubcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "BrandSubcategories",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    SubcategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandSubcategories", x => new { x.BrandId, x.SubcategoryId });
                    table.ForeignKey(
                        name: "FK_BrandSubcategories_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandSubcategories_Subcategories_SubcategoryId",
                        column: x => x.SubcategoryId,
                        principalTable: "Subcategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

          

            migrationBuilder.CreateIndex(
                name: "IX_BrandSubcategories_SubcategoryId",
                table: "BrandSubcategories",
                column: "SubcategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandSubcategories");

          
        }
    }
}
