using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelRouteAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableTravelRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TravelRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Origin = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelRoutes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TravelRoutes",
                columns: new[] { "Id", "Destination", "Origin", "Price" },
                values: new object[,]
                {
                    { 1, "BRC", "GRU", 10 },
                    { 2, "SCL", "BRC", 5 },
                    { 3, "CDG", "GRU", 75 },
                    { 4, "SCL", "GRU", 20 },
                    { 5, "ORL", "GRU", 56 },
                    { 6, "CDG", "ORL", 5 },
                    { 7, "ORL", "SCL", 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TravelRoutes");
        }
    }
}
