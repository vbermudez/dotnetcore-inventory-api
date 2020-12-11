using Microsoft.EntityFrameworkCore.Migrations;

namespace GoalSystemsAPI.Migrations
{
    public partial class AdddExpiredFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Expired",
                table: "InventoryItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expired",
                table: "InventoryItems");
        }
    }
}
