using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fixedWrongConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOptions_Shippings_ShippingId",
                table: "DeliveryOptions");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOptions_ShippingId",
                table: "DeliveryOptions");

            migrationBuilder.DropColumn(
                name: "ShippingId",
                table: "DeliveryOptions");

            migrationBuilder.CreateIndex(
                name: "IX_Shippings_DeliveryOptionsId",
                table: "Shippings",
                column: "DeliveryOptionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shippings_DeliveryOptions_DeliveryOptionsId",
                table: "Shippings",
                column: "DeliveryOptionsId",
                principalTable: "DeliveryOptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_DeliveryOptions_DeliveryOptionsId",
                table: "Shippings");

            migrationBuilder.DropIndex(
                name: "IX_Shippings_DeliveryOptionsId",
                table: "Shippings");

            migrationBuilder.AddColumn<int>(
                name: "ShippingId",
                table: "DeliveryOptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOptions_ShippingId",
                table: "DeliveryOptions",
                column: "ShippingId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOptions_Shippings_ShippingId",
                table: "DeliveryOptions",
                column: "ShippingId",
                principalTable: "Shippings",
                principalColumn: "Id");
        }
    }
}
