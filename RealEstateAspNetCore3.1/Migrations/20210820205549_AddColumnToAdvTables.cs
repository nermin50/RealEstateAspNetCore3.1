using Microsoft.EntityFrameworkCore.Migrations;

namespace RealEstateAspNetCore3._1.Migrations
{
    public partial class AddColumnToAdvTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "advertisements",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "advertisements");
        }
    }
}
