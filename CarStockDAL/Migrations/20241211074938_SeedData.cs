using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarStockDAL.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "CarId", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Toyota" },
                    { 2, 0, "BMW" },
                    { 3, 0, "Porsche" },
                    { 4, 0, "Mercedes" },
                    { 5, 0, "Audi" }
                });

            migrationBuilder.InsertData(
                table: "Colors",
                columns: new[] { "Id", "CarId", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Red" },
                    { 2, 0, "Blue" },
                    { 3, 0, "Black" },
                    { 4, 0, "White" },
                    { 5, 0, "Silver" },
                    { 6, 0, "Green" },
                    { 7, 0, "Yellow" },
                    { 8, 0, "Gray" }
                });

            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "Id", "CarId", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Corolla" },
                    { 2, 0, "Camry" },
                    { 3, 0, "RAV4" },
                    { 4, 0, "X5" },
                    { 5, 0, "3 Series" },
                    { 6, 0, "5 Series" },
                    { 7, 0, "911 Carrera" },
                    { 8, 0, "Cayenne" },
                    { 9, 0, "Macan" },
                    { 10, 0, "C-Class" },
                    { 11, 0, "E-Class" },
                    { 12, 0, "GLE" },
                    { 13, 0, "A4" },
                    { 14, 0, "Q5" },
                    { 15, 0, "A6" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Colors",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
