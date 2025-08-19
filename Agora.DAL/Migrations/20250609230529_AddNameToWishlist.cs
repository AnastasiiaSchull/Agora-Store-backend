using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToWishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Wishlists",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Wishlists");
        }
    }
}
