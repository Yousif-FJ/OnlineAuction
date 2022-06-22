using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionBackend.Infrastructure.Migrations
{
    public partial class RemoveUserAuctionRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Users_UserId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_UserId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Auctions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Auctions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_UserId",
                table: "Auctions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Users_UserId",
                table: "Auctions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
