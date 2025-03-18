using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashFlowly.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoTablasFaltantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Usuarios_UsuarioId",
                table: "Cuentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_CategoriasGastos_CategoriaId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Cuentas_CuentaId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Usuarios_UsuarioId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_CategoriasIngresos_CategoriaId",
                table: "Ingresos");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_Cuentas_CuentaId",
                table: "Ingresos");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_Usuarios_UsuarioId",
                table: "Ingresos");

            migrationBuilder.DropForeignKey(
                name: "FK_MetasFinancieras_Usuarios_UsuarioId",
                table: "MetasFinancieras");

            migrationBuilder.CreateTable(
                name: "CategoriasGastosPersonalizadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasGastosPersonalizadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriasGastosPersonalizadas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriasIngresosPersonalizadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasIngresosPersonalizadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriasIngresosPersonalizadas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasGastosPersonalizadas_UsuarioId",
                table: "CategoriasGastosPersonalizadas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasIngresosPersonalizadas_UsuarioId",
                table: "CategoriasIngresosPersonalizadas",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Usuarios_UsuarioId",
                table: "Cuentas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_CategoriasGastos_CategoriaId",
                table: "Gastos",
                column: "CategoriaId",
                principalTable: "CategoriasGastos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Cuentas_CuentaId",
                table: "Gastos",
                column: "CuentaId",
                principalTable: "Cuentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Usuarios_UsuarioId",
                table: "Gastos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_CategoriasIngresos_CategoriaId",
                table: "Ingresos",
                column: "CategoriaId",
                principalTable: "CategoriasIngresos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_Cuentas_CuentaId",
                table: "Ingresos",
                column: "CuentaId",
                principalTable: "Cuentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_Usuarios_UsuarioId",
                table: "Ingresos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MetasFinancieras_Usuarios_UsuarioId",
                table: "MetasFinancieras",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Usuarios_UsuarioId",
                table: "Cuentas");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_CategoriasGastos_CategoriaId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Cuentas_CuentaId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Usuarios_UsuarioId",
                table: "Gastos");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_CategoriasIngresos_CategoriaId",
                table: "Ingresos");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_Cuentas_CuentaId",
                table: "Ingresos");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingresos_Usuarios_UsuarioId",
                table: "Ingresos");

            migrationBuilder.DropForeignKey(
                name: "FK_MetasFinancieras_Usuarios_UsuarioId",
                table: "MetasFinancieras");

            migrationBuilder.DropTable(
                name: "CategoriasGastosPersonalizadas");

            migrationBuilder.DropTable(
                name: "CategoriasIngresosPersonalizadas");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Usuarios_UsuarioId",
                table: "Cuentas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_CategoriasGastos_CategoriaId",
                table: "Gastos",
                column: "CategoriaId",
                principalTable: "CategoriasGastos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Cuentas_CuentaId",
                table: "Gastos",
                column: "CuentaId",
                principalTable: "Cuentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Usuarios_UsuarioId",
                table: "Gastos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_CategoriasIngresos_CategoriaId",
                table: "Ingresos",
                column: "CategoriaId",
                principalTable: "CategoriasIngresos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_Cuentas_CuentaId",
                table: "Ingresos",
                column: "CuentaId",
                principalTable: "Cuentas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingresos_Usuarios_UsuarioId",
                table: "Ingresos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MetasFinancieras_Usuarios_UsuarioId",
                table: "MetasFinancieras",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
