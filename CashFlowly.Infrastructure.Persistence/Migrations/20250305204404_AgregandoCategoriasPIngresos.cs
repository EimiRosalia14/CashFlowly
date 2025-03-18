using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashFlowly.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoCategoriasPIngresos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_CategoriasIngresos_CategoriaId",
                table: "Ingresos");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "Ingresos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaPersonalizadaId",
                table: "Ingresos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_CategoriaPersonalizadaId",
                table: "Ingresos",
                column: "CategoriaPersonalizadaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_CategoriasIngresosPersonalizadas_CategoriaPersonalizadaId",
                table: "Ingresos",
                column: "CategoriaPersonalizadaId",
                principalTable: "CategoriasIngresosPersonalizadas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_CategoriasIngresos_CategoriaId",
                table: "Ingresos",
                column: "CategoriaId",
                principalTable: "CategoriasIngresos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_CategoriasIngresosPersonalizadas_CategoriaPersonalizadaId",
                table: "Ingresos");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_CategoriasIngresos_CategoriaId",
                table: "Ingresos");

            migrationBuilder.DropIndex(
                name: "IX_Ingresos_CategoriaPersonalizadaId",
                table: "Ingresos");

            migrationBuilder.DropColumn(
                name: "CategoriaPersonalizadaId",
                table: "Ingresos");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "Ingresos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_CategoriasIngresos_CategoriaId",
                table: "Ingresos",
                column: "CategoriaId",
                principalTable: "CategoriasIngresos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
