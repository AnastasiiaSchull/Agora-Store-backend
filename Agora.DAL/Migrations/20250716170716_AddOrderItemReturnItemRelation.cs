using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemReturnItemRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "ReturnItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnItems_OrderItemId",
                table: "ReturnItems",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnItems_OrderItems_OrderItemId",
                table: "ReturnItems",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnItems_OrderItems_OrderItemId",
                table: "ReturnItems");

            migrationBuilder.DropIndex(
                name: "IX_ReturnItems_OrderItemId",
                table: "ReturnItems");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "ReturnItems");
        }
    }
}
