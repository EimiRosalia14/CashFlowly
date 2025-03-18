using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashFlowly.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ponerrelacionesx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_CategoriasGastosPersonalizadas_CategoriaPersonalizadaId",
                table: "Gastos");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaPersonalizadaId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_CategoriasGastosPersonalizadas_CategoriaPersonalizadaId",
                table: "Gastos",
                column: "CategoriaPersonalizadaId",
                principalTable: "CategoriasGastosPersonalizadas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_CategoriasGastosPersonalizadas_CategoriaPersonalizadaId",
                table: "Gastos");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaPersonalizadaId",
                table: "Gastos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_CategoriasGastosPersonalizadas_CategoriaPersonalizadaId",
                table: "Gastos",
                column: "CategoriaPersonalizadaId",
                principalTable: "CategoriasGastosPersonalizadas",
                principalColumn: "Id");
        }
    }
}
