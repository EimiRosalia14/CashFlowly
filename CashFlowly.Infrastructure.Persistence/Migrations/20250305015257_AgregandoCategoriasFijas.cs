using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CashFlowly.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoCategoriasFijas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CategoriasGastos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Vivienda" },
                    { 2, "Alimentación" },
                    { 3, "Transporte" },
                    { 4, "Salud" },
                    { 5, "Entretenimiento" }
                });

            migrationBuilder.InsertData(
                table: "CategoriasIngresos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Salario" },
                    { 2, "Deudas cobradas" },
                    { 3, "Regalos / Donaciones" },
                    { 4, "Ventas personales" },
                    { 5, "Inversiones" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoriasGastos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CategoriasGastos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CategoriasGastos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CategoriasGastos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CategoriasGastos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CategoriasIngresos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CategoriasIngresos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CategoriasIngresos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CategoriasIngresos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CategoriasIngresos",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
