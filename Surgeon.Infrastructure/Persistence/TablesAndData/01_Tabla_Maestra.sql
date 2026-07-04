-- =========================================================================
-- SCRIPT DE CREACIÓN DE TABLAS MAESTRAS Y RELACIONALES - CIRUJANO GENERAL
-- CONVENCIONES DE AUDITORÍA Y ESTÁNDARES ADOPTADOS
-- =========================================================================

-- 1. Roles
CREATE TABLE dbo.Roles (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL UNIQUE,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL
);
GO

-- 2. Usuarios (Para administración del CMS y portal médico)
CREATE TABLE dbo.Usuarios (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    NombreCompleto VARCHAR(100) NOT NULL,
    Correo VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Telefono VARCHAR(20) NULL,
    IdRol INT NOT NULL,
    EsActivo BIT DEFAULT 1 NOT NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL,
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (IdRol) REFERENCES dbo.Roles(Id)
);
GO

-- 3. Especialidades Médicas
CREATE TABLE dbo.Especialidades (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Icono VARCHAR(50) NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL
);
GO

-- 4. Servicios Médicos y Procedimientos Quirúrgicos
CREATE TABLE dbo.ServiciosMedicos (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(150) NOT NULL,
    Slug VARCHAR(150) NOT NULL UNIQUE, -- Optimizado para SEO en frontend-web
    Resumen VARCHAR(300) NOT NULL,
    DescripcionDetallada NVARCHAR(MAX) NULL,
    TiempoEstimadoMinutos INT DEFAULT 45 NOT NULL,
    PrecioReferencial DECIMAL(10,2) NULL,
    Icono VARCHAR(50) NULL,
    IdEspecialidad INT NOT NULL,
    DestacadoEnWeb BIT DEFAULT 1 NOT NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL,
    CONSTRAINT FK_ServiciosMedicos_Especialidades FOREIGN KEY (IdEspecialidad) REFERENCES dbo.Especialidades(Id)
);
GO

-- 5. Pacientes
CREATE TABLE dbo.Pacientes (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    NumeroDocumento VARCHAR(20) NULL,
    Correo VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    FechaNacimiento DATE NULL,
    AntecedentesRelevantes NVARCHAR(1000) NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL
);
GO

-- 6. Estados de Cita
CREATE TABLE dbo.EstadosCita (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL, -- Pendiente, Confirmada, Atendida, Cancelada
    ColorHex VARCHAR(10) DEFAULT '#3B82F6' NOT NULL, -- Colores para Tailwind UI en CMS
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL
);
GO

-- 7. Citas Médicas
CREATE TABLE dbo.Citas (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdPaciente INT NOT NULL,
    IdServicioMedico INT NOT NULL,
    FechaHoraInicio DATETIME NOT NULL,
    FechaHoraFin DATETIME NOT NULL,
    IdEstadoCita INT NOT NULL,
    MotivoConsulta VARCHAR(500) NULL,
    NotasMedico NVARCHAR(MAX) NULL,
    PrecioConsulta DECIMAL(10,2) NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL,
    CONSTRAINT FK_Citas_Pacientes FOREIGN KEY (IdPaciente) REFERENCES dbo.Pacientes(Id),
    CONSTRAINT FK_Citas_ServiciosMedicos FOREIGN KEY (IdServicioMedico) REFERENCES dbo.ServiciosMedicos(Id),
    CONSTRAINT FK_Citas_EstadosCita FOREIGN KEY (IdEstadoCita) REFERENCES dbo.EstadosCita(Id)
);
GO

-- 8. Horarios de Atención Regulares
CREATE TABLE dbo.HorariosAtencion (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    DiaSemana INT NOT NULL, -- 1: Lunes, 2: Martes, ..., 7: Domingo
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    EsActivo BIT DEFAULT 1 NOT NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL
);
GO

-- 9. Bloqueos de Horario (Congresos, Vacaciones, Quirófano)
CREATE TABLE dbo.BloqueosHorario (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FechaInicio DATETIME NOT NULL,
    FechaFin DATETIME NOT NULL,
    Motivo VARCHAR(255) NOT NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL
);
GO

-- 10. Categorías del Blog Médico
CREATE TABLE dbo.CategoriasBlog (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Slug VARCHAR(100) NOT NULL UNIQUE,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL
);
GO

-- 11. Artículos del Blog Médico (SEO para Cirujano)
CREATE TABLE dbo.ArticulosBlog (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Titulo VARCHAR(200) NOT NULL,
    Slug VARCHAR(200) NOT NULL UNIQUE,
    Resumen VARCHAR(500) NOT NULL,
    ContenidoHtml NVARCHAR(MAX) NOT NULL,
    ImagenPortadaUrl VARCHAR(500) NULL,
    IdCategoria INT NOT NULL,
    FechaPublicacion DATETIME DEFAULT GETDATE() NOT NULL,
    PublicadoEnWeb BIT DEFAULT 1 NOT NULL,
    Vistas INT DEFAULT 0 NOT NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL,
    CONSTRAINT FK_ArticulosBlog_Categorias FOREIGN KEY (IdCategoria) REFERENCES dbo.CategoriasBlog(Id)
);
GO

-- 12. Testimonios y Casos de Éxito
CREATE TABLE dbo.Testimonios (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    NombrePaciente VARCHAR(100) NOT NULL,
    Procedimiento VARCHAR(150) NOT NULL,
    CalificacionEstrellas INT DEFAULT 5 NOT NULL,
    Comentario VARCHAR(1000) NOT NULL,
    FechaTestimonio DATE DEFAULT GETDATE() NOT NULL,
    AprobadoParaWeb BIT DEFAULT 1 NOT NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL
);
GO

-- 13. Mensajes de Contacto Web
CREATE TABLE dbo.MensajesContacto (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Correo VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20) NULL,
    Asunto VARCHAR(150) NULL,
    Mensaje VARCHAR(1000) NOT NULL,
    Leido BIT DEFAULT 0 NOT NULL,
    FechaEnvio DATETIME DEFAULT GETDATE() NOT NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL
);
GO
