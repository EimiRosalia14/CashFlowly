using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashFlowly.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ponerrelaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_CategoriasGastos_CategoriaId",
                table: "Gastos");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "Gastos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaIdP",
                table: "Gastos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoriaPersonalizadaId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_CategoriaPersonalizadaId",
                table: "Gastos",
                column: "CategoriaPersonalizadaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_CategoriasGastosPersonalizadas_CategoriaPersonalizadaId",
                table: "Gastos",
                column: "CategoriaPersonalizadaId",
                principalTable: "CategoriasGastosPersonalizadas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_CategoriasGastos_CategoriaId",
                table: "Gastos",
                column: "CategoriaId",
                principalTable: "CategoriasGastos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_CategoriasGastosPersonalizadas_CategoriaPersonalizadaId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_CategoriasGastos_CategoriaId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_CategoriaPersonalizadaId",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "CategoriaIdP",
                table: "Gastos");

            migrationBuilder.DropColumn(
                name: "CategoriaPersonalizadaId",
                table: "Gastos");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "Gastos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_CategoriasGastos_CategoriaId",
                table: "Gastos",
                column: "CategoriaId",
                principalTable: "CategoriasGastos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
