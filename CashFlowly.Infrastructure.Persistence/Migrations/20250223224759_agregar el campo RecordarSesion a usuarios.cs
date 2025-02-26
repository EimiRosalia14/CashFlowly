using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashFlowly.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class agregarelcampoRecordarSesionausuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Bloqueado",
                table: "Usuarios",
                newName: "Suspendido");

            migrationBuilder.AddColumn<bool>(
                name: "RecordarSesion",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordarSesion",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "Suspendido",
                table: "Usuarios",
                newName: "Bloqueado");
        }
    }
}
