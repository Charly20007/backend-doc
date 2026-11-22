using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Surgeon.Infrastructure.Persistence.Migraciones
{
    /// <inheritdoc />
    public partial class AgendaTurnos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "anticipacion_minima_horas",
                table: "configuracion_sitio",
                type: "integer",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "dias_maximos_reserva",
                table: "configuracion_sitio",
                type: "integer",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.AddColumn<int>(
                name: "duracion_turno_minutos",
                table: "configuracion_sitio",
                type: "integer",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.AddColumn<bool>(
                name: "reservas_online_activas",
                table: "configuracion_sitio",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ocupa_turno",
                table: "citas",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            // Las citas canceladas no ocupan turno
            migrationBuilder.Sql("UPDATE citas SET ocupa_turno = false WHERE id_estado_cita IN (SELECT id FROM estados_cita WHERE nombre = 'Cancelada');");

            migrationBuilder.CreateIndex(
                name: "ix_citas_turno_ocupado",
                table: "citas",
                column: "fecha_hora_inicio",
                unique: true,
                filter: "ocupa_turno AND NOT eliminado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_citas_turno_ocupado",
                table: "citas");

            migrationBuilder.DropColumn(
                name: "anticipacion_minima_horas",
                table: "configuracion_sitio");

            migrationBuilder.DropColumn(
                name: "dias_maximos_reserva",
                table: "configuracion_sitio");

            migrationBuilder.DropColumn(
                name: "duracion_turno_minutos",
                table: "configuracion_sitio");

            migrationBuilder.DropColumn(
                name: "reservas_online_activas",
                table: "configuracion_sitio");

            migrationBuilder.DropColumn(
                name: "ocupa_turno",
                table: "citas");
        }
    }
}
