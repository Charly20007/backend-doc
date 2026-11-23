using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Surgeon.Infrastructure.Persistence.Migraciones
{
    /// <inheritdoc />
    public partial class ReservaEnClinica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "reserva_clinica_activa",
                table: "configuracion_sitio",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "reserva_clinica_nombre",
                table: "configuracion_sitio",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reserva_clinica_telefono",
                table: "configuracion_sitio",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reserva_clinica_texto_busqueda",
                table: "configuracion_sitio",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reserva_clinica_url",
                table: "configuracion_sitio",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reserva_clinica_activa",
                table: "configuracion_sitio");

            migrationBuilder.DropColumn(
                name: "reserva_clinica_nombre",
                table: "configuracion_sitio");

            migrationBuilder.DropColumn(
                name: "reserva_clinica_telefono",
                table: "configuracion_sitio");

            migrationBuilder.DropColumn(
                name: "reserva_clinica_texto_busqueda",
                table: "configuracion_sitio");

            migrationBuilder.DropColumn(
                name: "reserva_clinica_url",
                table: "configuracion_sitio");
        }
    }
}
