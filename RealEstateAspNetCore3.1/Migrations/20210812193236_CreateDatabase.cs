using Microsoft.EntityFrameworkCore.Migrations;

namespace RealEstateAspNetCore3._1.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    CityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    StatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "districts",
                columns: table => new
                {
                    DistrictId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictName = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_districts", x => x.DistrictId);
                    table.ForeignKey(
                        name: "FK_districts_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tips",
                columns: table => new
                {
                    TypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tips", x => x.TypeId);
                    table.ForeignKey(
                        name: "FK_Tips_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "neighborhoods",
                columns: table => new
                {
                    NeighborhoodId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NeighborhoodName = table.Column<string>(nullable: true),
                    DistrictId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_neighborhoods", x => x.NeighborhoodId);
                    table.ForeignKey(
                        name: "FK_neighborhoods_districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "advertisements",
                columns: table => new
                {
                    AdvId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    NumOfRoom = table.Column<string>(nullable: true),
                    NumOfBath = table.Column<string>(nullable: true),
                    Credit = table.Column<bool>(nullable: false),
                    Area = table.Column<int>(nullable: false),
                    Floor = table.Column<int>(nullable: false),
                    Feature = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    Addres = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    DistrictId = table.Column<int>(nullable: false),
                    NeighborhoodId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    TipTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advertisements", x => x.AdvId);
                    table.ForeignKey(
                        name: "FK_advertisements_neighborhoods_NeighborhoodId",
                        column: x => x.NeighborhoodId,
                        principalTable: "neighborhoods",
                        principalColumn: "NeighborhoodId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_advertisements_Tips_TipTypeId",
                        column: x => x.TipTypeId,
                        principalTable: "Tips",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "advPhotos",
                columns: table => new
                {
                    AdvPhotoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvPhotoName = table.Column<string>(nullable: true),
                    AdvId = table.Column<int>(nullable: false),
                    AdvertisementAdvId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_advPhotos", x => x.AdvPhotoId);
                    table.ForeignKey(
                        name: "FK_advPhotos_advertisements_AdvertisementAdvId",
                        column: x => x.AdvertisementAdvId,
                        principalTable: "advertisements",
                        principalColumn: "AdvId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_advertisements_NeighborhoodId",
                table: "advertisements",
                column: "NeighborhoodId");

            migrationBuilder.CreateIndex(
                name: "IX_advertisements_TipTypeId",
                table: "advertisements",
                column: "TipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_advPhotos_AdvertisementAdvId",
                table: "advPhotos",
                column: "AdvertisementAdvId");

            migrationBuilder.CreateIndex(
                name: "IX_districts_CityId",
                table: "districts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_neighborhoods_DistrictId",
                table: "neighborhoods",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Tips_StatusId",
                table: "Tips",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "advPhotos");

            migrationBuilder.DropTable(
                name: "advertisements");

            migrationBuilder.DropTable(
                name: "neighborhoods");

            migrationBuilder.DropTable(
                name: "Tips");

            migrationBuilder.DropTable(
                name: "districts");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "cities");
        }
    }
}
