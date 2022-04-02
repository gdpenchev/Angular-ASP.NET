using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CuriousReadersData.Migrations
{
    public partial class RatingColumnRemovedFromBookEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Books",
                type: "float",
                nullable: true);
        }
    }
}
