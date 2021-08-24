using Microsoft.EntityFrameworkCore.Migrations;

namespace RealEstateAspNetCore3._1.Migrations
{
    public partial class addUserNameColumnToadvTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_advPhotos_advertisements_AdvertisementAdvId",
                table: "advPhotos");

            migrationBuilder.DropIndex(
                name: "IX_advPhotos_AdvertisementAdvId",
                table: "advPhotos");

            migrationBuilder.DropColumn(
                name: "AdvertisementAdvId",
                table: "advPhotos");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "advertisements",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_advPhotos_AdvId",
                table: "advPhotos",
                column: "AdvId");

            migrationBuilder.AddForeignKey(
                name: "FK_advPhotos_advertisements_AdvId",
                table: "advPhotos",
                column: "AdvId",
                principalTable: "advertisements",
                principalColumn: "AdvId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_advPhotos_advertisements_AdvId",
                table: "advPhotos");

            migrationBuilder.DropIndex(
                name: "IX_advPhotos_AdvId",
                table: "advPhotos");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "advertisements");

            migrationBuilder.AddColumn<int>(
                name: "AdvertisementAdvId",
                table: "advPhotos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_advPhotos_AdvertisementAdvId",
                table: "advPhotos",
                column: "AdvertisementAdvId");

            migrationBuilder.AddForeignKey(
                name: "FK_advPhotos_advertisements_AdvertisementAdvId",
                table: "advPhotos",
                column: "AdvertisementAdvId",
                principalTable: "advertisements",
                principalColumn: "AdvId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
