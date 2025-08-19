using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addedNewFieldToGiftCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "GiftCards",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "GiftCards");
        }
    }
}
