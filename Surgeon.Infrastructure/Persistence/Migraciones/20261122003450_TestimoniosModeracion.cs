using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Surgeon.Infrastructure.Persistence.Migraciones
{
    /// <inheritdoc />
    public partial class TestimoniosModeracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "autoriza_publicacion",
                table: "testimonios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "contacto_paciente",
                table: "testimonios",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "enviado_desde_web",
                table: "testimonios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "pendiente_revision",
                table: "testimonios",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "autoriza_publicacion",
                table: "testimonios");

            migrationBuilder.DropColumn(
                name: "contacto_paciente",
                table: "testimonios");

            migrationBuilder.DropColumn(
                name: "enviado_desde_web",
                table: "testimonios");

            migrationBuilder.DropColumn(
                name: "pendiente_revision",
                table: "testimonios");
        }
    }
}
