using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Surgeon.Infrastructure.Persistence.Migraciones
{
    /// <inheritdoc />
    public partial class BlogDestacadoYTextosPagina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "descripcion",
                table: "paginas_seo",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "titulo",
                table: "paginas_seo",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "titulo_secundario",
                table: "paginas_seo",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "destacado",
                table: "articulos_blog",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "descripcion",
                table: "paginas_seo");

            migrationBuilder.DropColumn(
                name: "titulo",
                table: "paginas_seo");

            migrationBuilder.DropColumn(
                name: "titulo_secundario",
                table: "paginas_seo");

            migrationBuilder.DropColumn(
                name: "destacado",
                table: "articulos_blog");
        }
    }
}
