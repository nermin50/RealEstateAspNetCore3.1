using Microsoft.EntityFrameworkCore.Migrations;

namespace RealEstateAspNetCore3._1.Migrations
{
    public partial class updateforeignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_advertisements_Tips_TipTypeId",
                table: "advertisements");

            migrationBuilder.DropIndex(
                name: "IX_advertisements_TipTypeId",
                table: "advertisements");

            migrationBuilder.DropColumn(
                name: "TipTypeId",
                table: "advertisements");

            migrationBuilder.CreateIndex(
                name: "IX_advertisements_TypeId",
                table: "advertisements",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_advertisements_Tips_TypeId",
                table: "advertisements",
                column: "TypeId",
                principalTable: "Tips",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_advertisements_Tips_TypeId",
                table: "advertisements");

            migrationBuilder.DropIndex(
                name: "IX_advertisements_TypeId",
                table: "advertisements");

            migrationBuilder.AddColumn<int>(
                name: "TipTypeId",
                table: "advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_advertisements_TipTypeId",
                table: "advertisements",
                column: "TipTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_advertisements_Tips_TipTypeId",
                table: "advertisements",
                column: "TipTypeId",
                principalTable: "Tips",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
