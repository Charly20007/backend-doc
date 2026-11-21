using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Surgeon.Infrastructure.Persistence.Migraciones
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bloqueos_horario",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    motivo = table.Column<string>(type: "text", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bloqueos_horario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categorias_blog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categorias_blog", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "configuracion_sitio",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_doctor = table.Column<string>(type: "text", nullable: false),
                    nombre_corto = table.Column<string>(type: "text", nullable: true),
                    especialidad = table.Column<string>(type: "text", nullable: true),
                    cmp = table.Column<string>(type: "text", nullable: true),
                    rne = table.Column<string>(type: "text", nullable: true),
                    foto_doctor_url = table.Column<string>(type: "text", nullable: true),
                    seguros_aliados = table.Column<string>(type: "text", nullable: true),
                    telefono_principal = table.Column<string>(type: "text", nullable: true),
                    telefono_secundario = table.Column<string>(type: "text", nullable: true),
                    whats_app = table.Column<string>(type: "text", nullable: true),
                    correo = table.Column<string>(type: "text", nullable: true),
                    direccion = table.Column<string>(type: "text", nullable: true),
                    distrito = table.Column<string>(type: "text", nullable: true),
                    ciudad = table.Column<string>(type: "text", nullable: true),
                    region = table.Column<string>(type: "text", nullable: true),
                    pais = table.Column<string>(type: "text", nullable: false),
                    google_maps_url = table.Column<string>(type: "text", nullable: true),
                    descripcion_pie = table.Column<string>(type: "text", nullable: true),
                    mostrar_emergencias = table.Column<bool>(type: "boolean", nullable: false),
                    texto_emergencias = table.Column<string>(type: "text", nullable: true),
                    facebook_url = table.Column<string>(type: "text", nullable: true),
                    instagram_url = table.Column<string>(type: "text", nullable: true),
                    tik_tok_url = table.Column<string>(type: "text", nullable: true),
                    you_tube_url = table.Column<string>(type: "text", nullable: true),
                    linked_in_url = table.Column<string>(type: "text", nullable: true),
                    site_url = table.Column<string>(type: "text", nullable: false),
                    meta_titulo_por_defecto = table.Column<string>(type: "text", nullable: true),
                    meta_descripcion_por_defecto = table.Column<string>(type: "text", nullable: true),
                    imagen_og_por_defecto_url = table.Column<string>(type: "text", nullable: true),
                    google_site_verification = table.Column<string>(type: "text", nullable: true),
                    google_analytics_id = table.Column<string>(type: "text", nullable: true),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_configuracion_sitio", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "especialidades",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: true),
                    icono = table.Column<string>(type: "text", nullable: true),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_especialidades", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "estados_cita",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    color_hex = table.Column<string>(type: "text", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_estados_cita", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "horarios_atencion",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dia_semana = table.Column<int>(type: "integer", nullable: false),
                    hora_inicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    hora_fin = table.Column<TimeSpan>(type: "time", nullable: false),
                    es_activo = table.Column<bool>(type: "boolean", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_horarios_atencion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mensajes_contacto",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    correo = table.Column<string>(type: "text", nullable: false),
                    telefono = table.Column<string>(type: "text", nullable: true),
                    asunto = table.Column<string>(type: "text", nullable: true),
                    mensaje = table.Column<string>(type: "text", nullable: false),
                    leido = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_envio = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mensajes_contacto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pacientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombres = table.Column<string>(type: "text", nullable: false),
                    apellidos = table.Column<string>(type: "text", nullable: false),
                    numero_documento = table.Column<string>(type: "text", nullable: true),
                    correo = table.Column<string>(type: "text", nullable: false),
                    telefono = table.Column<string>(type: "text", nullable: false),
                    fecha_nacimiento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    antecedentes_relevantes = table.Column<string>(type: "text", nullable: true),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pacientes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "paginas_legales",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    clave = table.Column<string>(type: "text", nullable: false),
                    ruta = table.Column<string>(type: "text", nullable: false),
                    titulo = table.Column<string>(type: "text", nullable: false),
                    contenido_html = table.Column<string>(type: "text", nullable: false),
                    publicada = table.Column<bool>(type: "boolean", nullable: false),
                    meta_titulo = table.Column<string>(type: "text", nullable: true),
                    meta_descripcion = table.Column<string>(type: "text", nullable: true),
                    imagen_og_url = table.Column<string>(type: "text", nullable: true),
                    indexable = table.Column<bool>(type: "boolean", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_paginas_legales", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "paginas_seo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    clave = table.Column<string>(type: "text", nullable: false),
                    ruta = table.Column<string>(type: "text", nullable: false),
                    nombre_pagina = table.Column<string>(type: "text", nullable: false),
                    meta_titulo = table.Column<string>(type: "text", nullable: true),
                    meta_descripcion = table.Column<string>(type: "text", nullable: true),
                    imagen_og_url = table.Column<string>(type: "text", nullable: true),
                    indexable = table.Column<bool>(type: "boolean", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_paginas_seo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "redirecciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ruta_origen = table.Column<string>(type: "text", nullable: false),
                    ruta_destino = table.Column<string>(type: "text", nullable: false),
                    es_permanente = table.Column<bool>(type: "boolean", nullable: false),
                    activa = table.Column<bool>(type: "boolean", nullable: false),
                    nota = table.Column<string>(type: "text", nullable: true),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_redirecciones", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "servicios_medicos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "text", nullable: false),
                    nombre_corto = table.Column<string>(type: "text", nullable: true),
                    slug = table.Column<string>(type: "text", nullable: false),
                    resumen = table.Column<string>(type: "text", nullable: false),
                    descripcion_detallada = table.Column<string>(type: "text", nullable: true),
                    tiempo_estimado_minutos = table.Column<int>(type: "integer", nullable: false),
                    precio_referencial = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    icono = table.Column<string>(type: "text", nullable: true),
                    id_especialidad = table.Column<int>(type: "integer", nullable: false),
                    destacado_en_web = table.Column<bool>(type: "boolean", nullable: false),
                    orden = table.Column<int>(type: "integer", nullable: false),
                    condicion_tratada = table.Column<string>(type: "text", nullable: true),
                    titulo_indicaciones = table.Column<string>(type: "text", nullable: true),
                    meta_titulo = table.Column<string>(type: "text", nullable: true),
                    meta_descripcion = table.Column<string>(type: "text", nullable: true),
                    imagen_og_url = table.Column<string>(type: "text", nullable: true),
                    indexable = table.Column<bool>(type: "boolean", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_servicios_medicos", x => x.id);
                    table.ForeignKey(
                        name: "fk_servicios_medicos_especialidades_id_especialidad",
                        column: x => x.id_especialidad,
                        principalTable: "especialidades",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_completo = table.Column<string>(type: "text", nullable: false),
                    correo = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    telefono = table.Column<string>(type: "text", nullable: true),
                    id_rol = table.Column<int>(type: "integer", nullable: false),
                    es_activo = table.Column<bool>(type: "boolean", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuarios", x => x.id);
                    table.ForeignKey(
                        name: "fk_usuarios_roles_id_rol",
                        column: x => x.id_rol,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "articulos_blog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    titulo = table.Column<string>(type: "text", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    resumen = table.Column<string>(type: "text", nullable: false),
                    contenido_html = table.Column<string>(type: "text", nullable: false),
                    imagen_portada_url = table.Column<string>(type: "text", nullable: true),
                    id_categoria = table.Column<int>(type: "integer", nullable: false),
                    id_servicio_medico = table.Column<int>(type: "integer", nullable: true),
                    autor = table.Column<string>(type: "text", nullable: false),
                    tiempo_lectura_minutos = table.Column<int>(type: "integer", nullable: false),
                    fecha_publicacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    publicado_en_web = table.Column<bool>(type: "boolean", nullable: false),
                    vistas = table.Column<int>(type: "integer", nullable: false),
                    meta_titulo = table.Column<string>(type: "text", nullable: true),
                    meta_descripcion = table.Column<string>(type: "text", nullable: true),
                    imagen_og_url = table.Column<string>(type: "text", nullable: true),
                    indexable = table.Column<bool>(type: "boolean", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_articulos_blog", x => x.id);
                    table.ForeignKey(
                        name: "fk_articulos_blog_categorias_blog_id_categoria",
                        column: x => x.id_categoria,
                        principalTable: "categorias_blog",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_articulos_blog_servicios_medicos_id_servicio_medico",
                        column: x => x.id_servicio_medico,
                        principalTable: "servicios_medicos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "citas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_paciente = table.Column<int>(type: "integer", nullable: false),
                    id_servicio_medico = table.Column<int>(type: "integer", nullable: false),
                    fecha_hora_inicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_hora_fin = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    id_estado_cita = table.Column<int>(type: "integer", nullable: false),
                    motivo_consulta = table.Column<string>(type: "text", nullable: true),
                    notas_medico = table.Column<string>(type: "text", nullable: true),
                    precio_consulta = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_citas", x => x.id);
                    table.ForeignKey(
                        name: "fk_citas_estados_cita_id_estado_cita",
                        column: x => x.id_estado_cita,
                        principalTable: "estados_cita",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_citas_pacientes_id_paciente",
                        column: x => x.id_paciente,
                        principalTable: "pacientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_citas_servicios_medicos_id_servicio_medico",
                        column: x => x.id_servicio_medico,
                        principalTable: "servicios_medicos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "servicios_contenido",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_servicio_medico = table.Column<int>(type: "integer", nullable: false),
                    tipo = table.Column<string>(type: "text", nullable: false),
                    titulo = table.Column<string>(type: "text", nullable: true),
                    texto = table.Column<string>(type: "text", nullable: false),
                    orden = table.Column<int>(type: "integer", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_servicios_contenido", x => x.id);
                    table.ForeignKey(
                        name: "fk_servicios_contenido_servicios_medicos_id_servicio_medico",
                        column: x => x.id_servicio_medico,
                        principalTable: "servicios_medicos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "testimonios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_paciente = table.Column<string>(type: "text", nullable: false),
                    procedimiento = table.Column<string>(type: "text", nullable: false),
                    id_servicio_medico = table.Column<int>(type: "integer", nullable: true),
                    calificacion_estrellas = table.Column<int>(type: "integer", nullable: false),
                    comentario = table.Column<string>(type: "text", nullable: false),
                    fecha_testimonio = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    aprobado_para_web = table.Column<bool>(type: "boolean", nullable: false),
                    usuario_modificacion = table.Column<string>(type: "text", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ip_modificacion = table.Column<string>(type: "text", nullable: false),
                    eliminado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_testimonios", x => x.id);
                    table.ForeignKey(
                        name: "fk_testimonios_servicios_medicos_id_servicio_medico",
                        column: x => x.id_servicio_medico,
                        principalTable: "servicios_medicos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_articulos_blog_id_categoria",
                table: "articulos_blog",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "ix_articulos_blog_id_servicio_medico",
                table: "articulos_blog",
                column: "id_servicio_medico");

            migrationBuilder.CreateIndex(
                name: "ix_articulos_blog_slug",
                table: "articulos_blog",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_categorias_blog_slug",
                table: "categorias_blog",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_citas_id_estado_cita",
                table: "citas",
                column: "id_estado_cita");

            migrationBuilder.CreateIndex(
                name: "ix_citas_id_paciente",
                table: "citas",
                column: "id_paciente");

            migrationBuilder.CreateIndex(
                name: "ix_citas_id_servicio_medico",
                table: "citas",
                column: "id_servicio_medico");

            migrationBuilder.CreateIndex(
                name: "ix_paginas_legales_clave",
                table: "paginas_legales",
                column: "clave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_paginas_seo_clave",
                table: "paginas_seo",
                column: "clave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_redirecciones_ruta_origen",
                table: "redirecciones",
                column: "ruta_origen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_roles_nombre",
                table: "roles",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_servicios_contenido_id_servicio_medico",
                table: "servicios_contenido",
                column: "id_servicio_medico");

            migrationBuilder.CreateIndex(
                name: "ix_servicios_medicos_id_especialidad",
                table: "servicios_medicos",
                column: "id_especialidad");

            migrationBuilder.CreateIndex(
                name: "ix_servicios_medicos_slug",
                table: "servicios_medicos",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_testimonios_id_servicio_medico",
                table: "testimonios",
                column: "id_servicio_medico");

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_correo",
                table: "usuarios",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_id_rol",
                table: "usuarios",
                column: "id_rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articulos_blog");

            migrationBuilder.DropTable(
                name: "bloqueos_horario");

            migrationBuilder.DropTable(
                name: "citas");

            migrationBuilder.DropTable(
                name: "configuracion_sitio");

            migrationBuilder.DropTable(
                name: "horarios_atencion");

            migrationBuilder.DropTable(
                name: "mensajes_contacto");

            migrationBuilder.DropTable(
                name: "paginas_legales");

            migrationBuilder.DropTable(
                name: "paginas_seo");

            migrationBuilder.DropTable(
                name: "redirecciones");

            migrationBuilder.DropTable(
                name: "servicios_contenido");

            migrationBuilder.DropTable(
                name: "testimonios");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "categorias_blog");

            migrationBuilder.DropTable(
                name: "estados_cita");

            migrationBuilder.DropTable(
                name: "pacientes");

            migrationBuilder.DropTable(
                name: "servicios_medicos");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "especialidades");
        }
    }
}
