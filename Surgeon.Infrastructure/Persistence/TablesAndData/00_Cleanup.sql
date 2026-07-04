-- =========================================================================
-- SCRIPT DE LIMPIEZA DE TABLAS Y CATÁLOGOS - CIRUJANO GENERAL
-- =========================================================================

IF OBJECT_ID('dbo.MensajesContacto', 'U') IS NOT NULL DROP TABLE dbo.MensajesContacto;
IF OBJECT_ID('dbo.Testimonios', 'U') IS NOT NULL DROP TABLE dbo.Testimonios;
IF OBJECT_ID('dbo.ArticulosBlog', 'U') IS NOT NULL DROP TABLE dbo.ArticulosBlog;
IF OBJECT_ID('dbo.CategoriasBlog', 'U') IS NOT NULL DROP TABLE dbo.CategoriasBlog;
IF OBJECT_ID('dbo.BloqueosHorario', 'U') IS NOT NULL DROP TABLE dbo.BloqueosHorario;
IF OBJECT_ID('dbo.HorariosAtencion', 'U') IS NOT NULL DROP TABLE dbo.HorariosAtencion;
IF OBJECT_ID('dbo.Citas', 'U') IS NOT NULL DROP TABLE dbo.Citas;
IF OBJECT_ID('dbo.EstadosCita', 'U') IS NOT NULL DROP TABLE dbo.EstadosCita;
IF OBJECT_ID('dbo.Pacientes', 'U') IS NOT NULL DROP TABLE dbo.Pacientes;
IF OBJECT_ID('dbo.ServiciosMedicos', 'U') IS NOT NULL DROP TABLE dbo.ServiciosMedicos;
IF OBJECT_ID('dbo.Especialidades', 'U') IS NOT NULL DROP TABLE dbo.Especialidades;
IF OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL DROP TABLE dbo.Usuarios;
IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL DROP TABLE dbo.Roles;
GO
