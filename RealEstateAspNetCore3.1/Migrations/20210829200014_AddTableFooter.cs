using Microsoft.EntityFrameworkCore.Migrations;

namespace RealEstateAspNetCore3._1.Migrations
{
    public partial class AddTableFooter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "footerLinks",
                columns: table => new
                {
                    FooterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    facebook = table.Column<string>(nullable: true),
                    twitter = table.Column<string>(nullable: true),
                    instagram = table.Column<string>(nullable: true),
                    youtube = table.Column<string>(nullable: true),
                    googlemap = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    address = table.Column<string>(nullable: true),
                    whatsapp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_footerLinks", x => x.FooterId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "footerLinks");
        }
    }
}
