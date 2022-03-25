using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionBackend.Infrastructure.Migrations
{
    public partial class AddPictureIdToItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoId",
                table: "Items",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Items");
        }
    }
}
