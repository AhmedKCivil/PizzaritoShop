using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaritoShop.Migrations
{
    /// <inheritdoc />
    public partial class DeletedUneccColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_OrdersTable_OrderListModelId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_PizzaOrder_PizzaOrderId",
                table: "CartItems");

            migrationBuilder.DropTable(
                name: "PizzaOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_PizzaOrderId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "PizzaOrderId",
                table: "CartItems");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "CartItem");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_OrderListModelId",
                table: "CartItem",
                newName: "IX_CartItem_OrderListModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem",
                column: "CartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_OrdersTable_OrderListModelId",
                table: "CartItem",
                column: "OrderListModelId",
                principalTable: "OrdersTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_OrdersTable_OrderListModelId",
                table: "CartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem");

            migrationBuilder.RenameTable(
                name: "CartItem",
                newName: "CartItems");

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_OrderListModelId",
                table: "CartItems",
                newName: "IX_CartItems_OrderListModelId");

            migrationBuilder.AddColumn<int>(
                name: "PizzaOrderId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems",
                column: "CartItemId");

            migrationBuilder.CreateTable(
                name: "PizzaOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PizzaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PizzaPrice = table.Column<double>(type: "float", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaOrder", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_PizzaOrderId",
                table: "CartItems",
                column: "PizzaOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_OrdersTable_OrderListModelId",
                table: "CartItems",
                column: "OrderListModelId",
                principalTable: "OrdersTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_PizzaOrder_PizzaOrderId",
                table: "CartItems",
                column: "PizzaOrderId",
                principalTable: "PizzaOrder",
                principalColumn: "Id");
        }
    }
}
