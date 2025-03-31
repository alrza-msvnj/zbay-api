using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Shop_ShopId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ShopId",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_Shop_OwnerId",
                table: "Shop",
                column: "OwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shop_User_OwnerId",
                table: "Shop",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shop_User_OwnerId",
                table: "Shop");

            migrationBuilder.DropIndex(
                name: "IX_Shop_OwnerId",
                table: "Shop");

            migrationBuilder.CreateIndex(
                name: "IX_User_ShopId",
                table: "User",
                column: "ShopId",
                unique: true,
                filter: "[ShopId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Shop_ShopId",
                table: "User",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id");
        }
    }
}
