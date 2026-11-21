-- =========================================================================
-- FASE 1: SEO ADMINISTRABLE DESDE EL CMS - CIRUJANO GENERAL
-- -------------------------------------------------------------------------
-- Script idempotente (se puede ejecutar varias veces):
--   * Base nueva:      00_Cleanup -> 01_Tabla_Maestra -> 02_Carga_Inicial -> 03_Fase1_SEO
--   * Base existente:  ejecutar solo este script
--
-- El usuario administrador NO se crea aquí: la API lo crea al iniciar si no
-- existe ningún usuario (ver sección "AdminInicial" en appsettings).
-- =========================================================================

-- 1. NUEVAS COLUMNAS EN TABLAS EXISTENTES ---------------------------------

-- 1.1 ServiciosMedicos: contenido de página y SEO
IF COL_LENGTH('dbo.ServiciosMedicos', 'NombreCorto') IS NULL
    ALTER TABLE dbo.ServiciosMedicos ADD NombreCorto NVARCHAR(80) NULL;
IF COL_LENGTH('dbo.ServiciosMedicos', 'Orden') IS NULL
    ALTER TABLE dbo.ServiciosMedicos ADD Orden INT NOT NULL CONSTRAINT DF_ServiciosMedicos_Orden DEFAULT 0;
IF COL_LENGTH('dbo.ServiciosMedicos', 'CondicionTratada') IS NULL
    ALTER TABLE dbo.ServiciosMedicos ADD CondicionTratada NVARCHAR(150) NULL;
IF COL_LENGTH('dbo.ServiciosMedicos', 'TituloIndicaciones') IS NULL
    ALTER TABLE dbo.ServiciosMedicos ADD TituloIndicaciones NVARCHAR(150) NULL;
IF COL_LENGTH('dbo.ServiciosMedicos', 'MetaTitulo') IS NULL
    ALTER TABLE dbo.ServiciosMedicos ADD MetaTitulo NVARCHAR(70) NULL;
IF COL_LENGTH('dbo.ServiciosMedicos', 'MetaDescripcion') IS NULL
    ALTER TABLE dbo.ServiciosMedicos ADD MetaDescripcion NVARCHAR(170) NULL;
IF COL_LENGTH('dbo.ServiciosMedicos', 'ImagenOgUrl') IS NULL
    ALTER TABLE dbo.ServiciosMedicos ADD ImagenOgUrl NVARCHAR(500) NULL;
IF COL_LENGTH('dbo.ServiciosMedicos', 'Indexable') IS NULL
    ALTER TABLE dbo.ServiciosMedicos ADD Indexable BIT NOT NULL CONSTRAINT DF_ServiciosMedicos_Indexable DEFAULT 1;
GO

-- 1.2 ArticulosBlog: procedimiento relacionado, autor, lectura y SEO
IF COL_LENGTH('dbo.ArticulosBlog', 'IdServicioMedico') IS NULL
BEGIN
    ALTER TABLE dbo.ArticulosBlog ADD IdServicioMedico INT NULL;
    ALTER TABLE dbo.ArticulosBlog ADD CONSTRAINT FK_ArticulosBlog_ServiciosMedicos
        FOREIGN KEY (IdServicioMedico) REFERENCES dbo.ServiciosMedicos(Id);
END
IF COL_LENGTH('dbo.ArticulosBlog', 'Autor') IS NULL
    ALTER TABLE dbo.ArticulosBlog ADD Autor NVARCHAR(100) NOT NULL CONSTRAINT DF_ArticulosBlog_Autor DEFAULT N'Dr. Juan Carlos Marcos Enriquez';
IF COL_LENGTH('dbo.ArticulosBlog', 'TiempoLecturaMinutos') IS NULL
    ALTER TABLE dbo.ArticulosBlog ADD TiempoLecturaMinutos INT NOT NULL CONSTRAINT DF_ArticulosBlog_TiempoLectura DEFAULT 5;
IF COL_LENGTH('dbo.ArticulosBlog', 'MetaTitulo') IS NULL
    ALTER TABLE dbo.ArticulosBlog ADD MetaTitulo NVARCHAR(70) NULL;
IF COL_LENGTH('dbo.ArticulosBlog', 'MetaDescripcion') IS NULL
    ALTER TABLE dbo.ArticulosBlog ADD MetaDescripcion NVARCHAR(170) NULL;
IF COL_LENGTH('dbo.ArticulosBlog', 'ImagenOgUrl') IS NULL
    ALTER TABLE dbo.ArticulosBlog ADD ImagenOgUrl NVARCHAR(500) NULL;
IF COL_LENGTH('dbo.ArticulosBlog', 'Indexable') IS NULL
    ALTER TABLE dbo.ArticulosBlog ADD Indexable BIT NOT NULL CONSTRAINT DF_ArticulosBlog_Indexable DEFAULT 1;
GO

-- 1.3 Testimonios: procedimiento relacionado
IF COL_LENGTH('dbo.Testimonios', 'IdServicioMedico') IS NULL
BEGIN
    ALTER TABLE dbo.Testimonios ADD IdServicioMedico INT NULL;
    ALTER TABLE dbo.Testimonios ADD CONSTRAINT FK_Testimonios_ServiciosMedicos
        FOREIGN KEY (IdServicioMedico) REFERENCES dbo.ServiciosMedicos(Id);
END
GO

-- 2. NUEVAS TABLAS --------------------------------------------------------

-- 2.1 Bloques de contenido de cada procedimiento
--     Tipo: Introduccion | Indicacion | Paso | Recuperacion | Pregunta
IF OBJECT_ID('dbo.ServiciosContenido', 'U') IS NULL
CREATE TABLE dbo.ServiciosContenido (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdServicioMedico INT NOT NULL,
    Tipo VARCHAR(20) NOT NULL,
    Titulo NVARCHAR(200) NULL,
    Texto NVARCHAR(2000) NOT NULL,
    Orden INT DEFAULT 0 NOT NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL,
    CONSTRAINT FK_ServiciosContenido_ServiciosMedicos FOREIGN KEY (IdServicioMedico) REFERENCES dbo.ServiciosMedicos(Id),
    CONSTRAINT CK_ServiciosContenido_Tipo CHECK (Tipo IN ('Introduccion', 'Indicacion', 'Paso', 'Recuperacion', 'Pregunta'))
);
GO

-- 2.2 Configuración global del sitio (una sola fila)
IF OBJECT_ID('dbo.ConfiguracionSitio', 'U') IS NULL
CREATE TABLE dbo.ConfiguracionSitio (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    NombreDoctor NVARCHAR(120) NOT NULL,
    NombreCorto NVARCHAR(60) NULL,
    Especialidad NVARCHAR(150) NULL,
    Cmp VARCHAR(20) NULL,
    Rne VARCHAR(20) NULL,
    FotoDoctorUrl NVARCHAR(500) NULL,
    SegurosAliados NVARCHAR(200) NULL,
    TelefonoPrincipal VARCHAR(30) NULL,
    TelefonoSecundario VARCHAR(30) NULL,
    WhatsApp VARCHAR(30) NULL,
    Correo VARCHAR(100) NULL,
    Direccion NVARCHAR(200) NULL,
    Distrito NVARCHAR(80) NULL,
    Ciudad NVARCHAR(80) NULL,
    Region NVARCHAR(80) NULL,
    Pais CHAR(2) DEFAULT 'PE' NOT NULL,
    GoogleMapsUrl NVARCHAR(1000) NULL,
    FacebookUrl NVARCHAR(300) NULL,
    InstagramUrl NVARCHAR(300) NULL,
    TikTokUrl NVARCHAR(300) NULL,
    YouTubeUrl NVARCHAR(300) NULL,
    LinkedInUrl NVARCHAR(300) NULL,
    SiteUrl VARCHAR(200) NOT NULL,
    MetaTituloPorDefecto NVARCHAR(70) NULL,
    MetaDescripcionPorDefecto NVARCHAR(170) NULL,
    ImagenOgPorDefectoUrl NVARCHAR(500) NULL,
    GoogleSiteVerification VARCHAR(100) NULL,
    GoogleAnalyticsId VARCHAR(30) NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL
);
GO

-- 2.3 SEO de páginas fijas
IF OBJECT_ID('dbo.PaginasSeo', 'U') IS NULL
CREATE TABLE dbo.PaginasSeo (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Clave VARCHAR(50) NOT NULL UNIQUE,
    Ruta VARCHAR(200) NOT NULL,
    NombrePagina NVARCHAR(100) NOT NULL,
    MetaTitulo NVARCHAR(70) NULL,
    MetaDescripcion NVARCHAR(170) NULL,
    ImagenOgUrl NVARCHAR(500) NULL,
    Indexable BIT DEFAULT 1 NOT NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL
);
GO

-- 2.4 Redirecciones de URLs antiguas (301)
IF OBJECT_ID('dbo.Redirecciones', 'U') IS NULL
CREATE TABLE dbo.Redirecciones (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    RutaOrigen VARCHAR(300) NOT NULL UNIQUE,
    RutaDestino VARCHAR(300) NOT NULL,
    EsPermanente BIT DEFAULT 1 NOT NULL,
    Activa BIT DEFAULT 1 NOT NULL,
    Nota NVARCHAR(200) NULL,
    UsuarioModificacion VARCHAR(20) DEFAULT 'UsrSurgeon' NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE() NOT NULL,
    IPModificacion VARCHAR(15) DEFAULT '::1' NOT NULL,
    Eliminado BIT DEFAULT 0 NOT NULL
);
GO

-- 3. DATOS INICIALES -------------------------------------------------------
-- ⚠️ Teléfonos, CMP/RNE y dirección son datos de ejemplo: corregirlos desde el CMS.

IF NOT EXISTS (SELECT 1 FROM dbo.ConfiguracionSitio)
INSERT INTO dbo.ConfiguracionSitio (
    NombreDoctor, NombreCorto, Especialidad, Cmp, Rne, FotoDoctorUrl, SegurosAliados,
    TelefonoPrincipal, TelefonoSecundario, Correo, Direccion, Distrito, Ciudad, Region, Pais,
    SiteUrl, MetaTituloPorDefecto, MetaDescripcionPorDefecto, ImagenOgPorDefectoUrl)
VALUES (
    N'Dr. Juan Carlos Marcos Enriquez', N'Dr. Juan Marcos', N'Especialista en Cirugía General y del Aparato Digestivo',
    '45892', '23104', N'/WhatsApp Image 2026-07-02 at 12.36.06 PM (1).jpeg', N'Pacífico, Rímac, Mapfre y Sanitas',
    '+51 1 999 8887', '+51 987 654 321', 'consultas@cirujanogeneral.pe',
    N'Av. Javier Prado Este 1234, Consultorio 502', N'San Isidro', N'Lima', N'Lima', 'PE',
    'https://cirujanogeneral.pe',
    N'Dr. Juan Carlos Marcos Enriquez | Cirugía General & Laparoscópica',
    N'Especialista en Cirugía General y del Aparato Digestivo por Mínima Invasión. Cirugía de vesícula, hernias, apendicitis y laparoscopía avanzada en Lima, Perú.',
    N'/WhatsApp Image 2026-07-02 at 12.36.06 PM (2).jpeg');
GO

MERGE dbo.PaginasSeo AS destino
USING (VALUES
    ('inicio', '/', N'Portada', N'Cirugía General y Laparoscópica en Lima | Dr. Juan Carlos Marcos',
        N'Tratamiento quirúrgico avanzado de vesícula, hernias abdominales, apendicitis y patologías digestivas con técnicas de mínima invasión y rápida recuperación.'),
    ('procedimientos', '/procedimientos', N'Listado de procedimientos', N'Procedimientos Quirúrgicos en Lima',
        N'Cirugía de vesícula, hernias, apendicitis, lipomas y consulta de valoración con cirugía laparoscópica de mínima invasión en San Isidro, Lima.'),
    ('blog', '/blog', N'Blog médico', N'Blog de Cirugía General y Salud Digestiva',
        N'Artículos del Dr. Juan Carlos Marcos sobre vesícula, hernias, apendicitis, recuperación postoperatoria y salud digestiva.'),
    ('perfil-doctor', '/dr-juan-carlos-marcos', N'Perfil del doctor', N'Dr. Juan Carlos Marcos Enriquez – Cirujano General en Lima',
        N'Conoce al Dr. Juan Carlos Marcos Enriquez, cirujano general y del aparato digestivo, especialista en cirugía laparoscópica de mínima invasión en San Isidro, Lima.'),
    ('contacto', '/contacto', N'Contacto', N'Contacto y Ubicación del Consultorio en San Isidro',
        N'Consultorio del Dr. Juan Carlos Marcos Enriquez en San Isidro, Lima. Teléfonos, horarios de atención, cómo llegar y reserva de citas.')
) AS origen (Clave, Ruta, NombrePagina, MetaTitulo, MetaDescripcion)
ON destino.Clave = origen.Clave
WHEN NOT MATCHED THEN
    INSERT (Clave, Ruta, NombrePagina, MetaTitulo, MetaDescripcion)
    VALUES (origen.Clave, origen.Ruta, origen.NombrePagina, origen.MetaTitulo, origen.MetaDescripcion);
GO

-- 3.1 Datos SEO y de contenido de cada procedimiento
UPDATE dbo.ServiciosMedicos SET NombreCorto = N'Cirugía de Vesícula', Resumen = N'Extirpación de la vesícula biliar por mínima invasión. Rápida recuperación en 24 a 48 horas sin cicatrices visibles.', CondicionTratada = N'Colelitiasis (cálculos en la vesícula)', TituloIndicaciones = N'¿Cuándo se recomienda operar la vesícula?', MetaTitulo = N'Cirugía de Vesícula Laparoscópica en Lima', MetaDescripcion = N'Colecistectomía laparoscópica para cálculos en la vesícula: síntomas, cómo es la operación, recuperación y precio referencial. Agenda tu evaluación en San Isidro.', Orden = 1 WHERE Slug = N'cirugia-de-vesicula-laparoscopica';
UPDATE dbo.ServiciosMedicos SET NombreCorto = N'Cirugía de Hernia', Resumen = N'Corrección quirúrgica con colocación de malla biocompatible de alta calidad para evitar recurrencias y aliviar el dolor al instante.', CondicionTratada = N'Hernia de pared abdominal', TituloIndicaciones = N'Síntomas de una hernia', MetaTitulo = N'Cirugía de Hernia Inguinal y Umbilical en Lima', MetaDescripcion = N'Reparación de hernia inguinal, umbilical o incisional con malla y técnica laparoscópica o abierta. Síntomas, recuperación y precio referencial en San Isidro.', Orden = 2 WHERE Slug = N'cirugia-de-hernia-inguinal-umbilical';
UPDATE dbo.ServiciosMedicos SET NombreCorto = N'Cirugía de Apéndice', Resumen = N'Tratamiento seguro y oportuno para la apendicitis aguda con incisiones milimétricas, mínima estancia y recuperación inmediata.', CondicionTratada = N'Apendicitis aguda', TituloIndicaciones = N'Síntomas de apendicitis', MetaTitulo = N'Cirugía de Apendicitis Laparoscópica en Lima', MetaDescripcion = N'Apendicectomía laparoscópica de urgencia: síntomas de apendicitis, cómo es la operación y recuperación. Atención de emergencias quirúrgicas en Lima.', Orden = 3 WHERE Slug = N'cirugia-de-apendice-laparoscopica';
UPDATE dbo.ServiciosMedicos SET NombreCorto = N'Consulta de Valoración', Resumen = N'Evaluación médica integral para diagnóstico preciso, revisión ecográfica/tomográfica y planificación personalizada de cirugía.', CondicionTratada = NULL, TituloIndicaciones = N'¿Cuándo pedir una consulta?', MetaTitulo = N'Consulta con Cirujano General en San Isidro', MetaDescripcion = N'Consulta de valoración quirúrgica y segunda opinión con cirujano general: revisión de ecografías y tomografías, diagnóstico y plan de tratamiento.', Orden = 4 WHERE Slug = N'consulta-valoracion-quirurgica';
UPDATE dbo.ServiciosMedicos SET NombreCorto = N'Lipomas y Quistes', Resumen = N'Procedimiento ambulatorio indoloro con anestesia local para lesiones en tejidos blandos con el mejor resultado estético.', CondicionTratada = N'Lipomas y quistes de piel', TituloIndicaciones = N'¿Cuándo conviene retirarlos?', MetaTitulo = N'Extirpación de Lipomas y Quistes Sebáceos en Lima', MetaDescripcion = N'Retiro de lipomas, quistes sebáceos y lunares con anestesia local, de forma ambulatoria y con cuidado estético. Agenda tu evaluación en San Isidro.', Orden = 5 WHERE Slug = N'cirugia-lipomas-y-quistes';
GO

-- 3.2 Bloques de contenido (solo si el procedimiento aún no tiene contenido)
IF NOT EXISTS (SELECT 1 FROM dbo.ServiciosContenido c JOIN dbo.ServiciosMedicos s ON s.Id = c.IdServicioMedico WHERE s.Slug = N'cirugia-de-vesicula-laparoscopica')
BEGIN
    DECLARE @IdServicio1 INT = (SELECT Id FROM dbo.ServiciosMedicos WHERE Slug = N'cirugia-de-vesicula-laparoscopica');
    INSERT INTO dbo.ServiciosContenido (IdServicioMedico, Tipo, Titulo, Texto, Orden) VALUES
    (@IdServicio1, 'Introduccion', NULL, N'La colecistectomía laparoscópica es la cirugía para extirpar la vesícula biliar cuando presenta cálculos (piedras), inflamación o pólipos. Es el tratamiento definitivo de la colelitiasis: los cálculos no desaparecen con dietas ni medicamentos.', 1),
    (@IdServicio1, 'Introduccion', NULL, N'Se realiza a través de pequeñas incisiones de 5 a 10 mm con una cámara de alta definición, lo que permite menos dolor, una estancia hospitalaria corta y un retorno rápido a las actividades diarias. El cuerpo funciona con normalidad sin vesícula: el hígado sigue produciendo la bilis necesaria para la digestión.', 2),
    (@IdServicio1, 'Indicacion', NULL, N'Dolor en la parte superior derecha del abdomen después de comer, sobre todo comidas grasas (cólico biliar).', 1),
    (@IdServicio1, 'Indicacion', NULL, N'Náuseas, vómitos, hinchazón o pesadez frecuentes tras las comidas.', 2),
    (@IdServicio1, 'Indicacion', NULL, N'Cálculos o barro biliar confirmados por ecografía, aunque den pocas molestias.', 3),
    (@IdServicio1, 'Indicacion', NULL, N'Episodios previos de colecistitis (vesícula inflamada) o pancreatitis biliar.', 4),
    (@IdServicio1, 'Indicacion', NULL, N'Pólipos vesiculares de tamaño significativo.', 5),
    (@IdServicio1, 'Indicacion', NULL, N'Coloración amarilla de piel u ojos (ictericia): requiere evaluación urgente.', 6),
    (@IdServicio1, 'Paso', N'Evaluación y exámenes', N'Consulta, ecografía abdominal y análisis de sangre preoperatorios. Se revisa el riesgo quirúrgico de forma individual.', 1),
    (@IdServicio1, 'Paso', N'Cirugía laparoscópica', N'Bajo anestesia general, mediante 3 o 4 pequeñas incisiones, se retira la vesícula con sus cálculos. Dura alrededor de 1 hora.', 2),
    (@IdServicio1, 'Paso', N'Alta temprana', N'La mayoría de pacientes camina el mismo día y se va a casa en 24 horas, con indicaciones de dieta y analgesia.', 3),
    (@IdServicio1, 'Paso', N'Control postoperatorio', N'Controles para revisar las heridas y el resultado de patología de la vesícula.', 4),
    (@IdServicio1, 'Recuperacion', NULL, N'Caminar desde el mismo día de la cirugía.', 1),
    (@IdServicio1, 'Recuperacion', NULL, N'Dieta baja en grasas las primeras semanas, con retorno progresivo a la alimentación normal.', 2),
    (@IdServicio1, 'Recuperacion', NULL, N'Trabajo de oficina en aproximadamente 7 días.', 3),
    (@IdServicio1, 'Recuperacion', NULL, N'Evitar cargar peso o hacer ejercicio intenso durante 3 a 4 semanas.', 4),
    (@IdServicio1, 'Pregunta', N'¿Se puede vivir normalmente sin vesícula?', N'Sí. La vesícula solo almacena la bilis; tras la cirugía el hígado la sigue produciendo y la vierte directamente al intestino. La gran mayoría de personas no nota diferencias a largo plazo.', 1),
    (@IdServicio1, 'Pregunta', N'¿Los cálculos se pueden eliminar con medicamentos o dietas?', N'No de forma confiable. Las dietas pueden reducir los síntomas, pero los cálculos permanecen y pueden causar complicaciones como colecistitis o pancreatitis. La cirugía es el tratamiento definitivo.', 2),
    (@IdServicio1, 'Pregunta', N'¿Cuánto dura la operación de vesícula?', N'Habitualmente alrededor de una hora, y el alta suele darse en 24 horas.', 3),
    (@IdServicio1, 'Pregunta', N'¿Qué pasa si no me opero?', N'Los síntomas tienden a repetirse y aumentan el riesgo de complicaciones que pueden requerir una cirugía de urgencia, más compleja que una programada.', 4);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.ServiciosContenido c JOIN dbo.ServiciosMedicos s ON s.Id = c.IdServicioMedico WHERE s.Slug = N'cirugia-de-hernia-inguinal-umbilical')
BEGIN
    DECLARE @IdServicio2 INT = (SELECT Id FROM dbo.ServiciosMedicos WHERE Slug = N'cirugia-de-hernia-inguinal-umbilical');
    INSERT INTO dbo.ServiciosContenido (IdServicioMedico, Tipo, Titulo, Texto, Orden) VALUES
    (@IdServicio2, 'Introduccion', NULL, N'Una hernia es la salida de grasa o intestino a través de un punto débil de la pared abdominal. Se nota como un bulto en la ingle, el ombligo o una cicatriz previa, que suele aumentar al toser, cargar peso o estar de pie.', 1),
    (@IdServicio2, 'Introduccion', NULL, N'Las hernias no se curan solas ni con fajas: con el tiempo tienden a crecer. La reparación quirúrgica con malla refuerza la pared abdominal y reduce de forma importante el riesgo de que la hernia vuelva a aparecer.', 2),
    (@IdServicio2, 'Indicacion', NULL, N'Bulto en la ingle, el ombligo o sobre una cicatriz que aparece al hacer esfuerzo y desaparece al acostarse.', 1),
    (@IdServicio2, 'Indicacion', NULL, N'Dolor, ardor o sensación de peso en la zona, sobre todo al final del día.', 2),
    (@IdServicio2, 'Indicacion', NULL, N'Molestia al toser, levantar objetos o hacer ejercicio.', 3),
    (@IdServicio2, 'Indicacion', NULL, N'Urgencia: bulto duro, doloroso que no se reduce, con náuseas o vómitos (posible hernia atascada).', 4),
    (@IdServicio2, 'Paso', N'Evaluación', N'Examen físico y, si es necesario, ecografía para definir el tipo y tamaño de la hernia.', 1),
    (@IdServicio2, 'Paso', N'Elección de técnica', N'Según el caso, reparación laparoscópica (pequeñas incisiones) o abierta, ambas con malla.', 2),
    (@IdServicio2, 'Paso', N'Cirugía', N'Se devuelve el contenido herniado a su lugar y se coloca una malla que refuerza la pared. Dura alrededor de 1 hora.', 3),
    (@IdServicio2, 'Paso', N'Alta y controles', N'En la mayoría de casos el alta es el mismo día o al día siguiente, con controles programados.', 4),
    (@IdServicio2, 'Recuperacion', NULL, N'Caminar desde el mismo día.', 1),
    (@IdServicio2, 'Recuperacion', NULL, N'Actividades ligeras y trabajo de oficina en 7 a 10 días.', 2),
    (@IdServicio2, 'Recuperacion', NULL, N'Evitar cargar peso y esfuerzos abdominales durante 4 a 6 semanas.', 3),
    (@IdServicio2, 'Recuperacion', NULL, N'Dolor controlable con analgésicos comunes.', 4),
    (@IdServicio2, 'Pregunta', N'¿Una hernia puede desaparecer sola?', N'No. Las fajas pueden aliviar molestias, pero no cierran el defecto de la pared abdominal; la única solución definitiva es la cirugía.', 1),
    (@IdServicio2, 'Pregunta', N'¿La malla es segura?', N'Sí. Las mallas modernas son biocompatibles y son el estándar internacional porque disminuyen notablemente la recurrencia de la hernia.', 2),
    (@IdServicio2, 'Pregunta', N'¿Cuándo es una urgencia?', N'Si el bulto se pone duro, muy doloroso, cambia de color o no regresa al acostarse, especialmente con vómitos. En ese caso hay que acudir de inmediato.', 3);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.ServiciosContenido c JOIN dbo.ServiciosMedicos s ON s.Id = c.IdServicioMedico WHERE s.Slug = N'cirugia-de-apendice-laparoscopica')
BEGIN
    DECLARE @IdServicio3 INT = (SELECT Id FROM dbo.ServiciosMedicos WHERE Slug = N'cirugia-de-apendice-laparoscopica');
    INSERT INTO dbo.ServiciosContenido (IdServicioMedico, Tipo, Titulo, Texto, Orden) VALUES
    (@IdServicio3, 'Introduccion', NULL, N'La apendicitis es la inflamación del apéndice y es una de las urgencias quirúrgicas más frecuentes. Su tratamiento es la apendicectomía: la extirpación del apéndice.', 1),
    (@IdServicio3, 'Introduccion', NULL, N'Operar a tiempo es clave. Si la apendicitis avanza, el apéndice puede perforarse y causar una infección en el abdomen (peritonitis). Por vía laparoscópica, la cirugía se realiza con pequeñas incisiones y una recuperación más rápida.', 2),
    (@IdServicio3, 'Indicacion', NULL, N'Dolor que empieza alrededor del ombligo y se desplaza a la parte baja derecha del abdomen.', 1),
    (@IdServicio3, 'Indicacion', NULL, N'Dolor que empeora al caminar, toser o saltar.', 2),
    (@IdServicio3, 'Indicacion', NULL, N'Pérdida de apetito, náuseas o vómitos.', 3),
    (@IdServicio3, 'Indicacion', NULL, N'Fiebre leve que puede aumentar con las horas.', 4),
    (@IdServicio3, 'Indicacion', NULL, N'Ante estos síntomas, no tomar analgésicos ni laxantes y acudir a evaluación médica.', 5),
    (@IdServicio3, 'Paso', N'Diagnóstico rápido', N'Examen físico, análisis de sangre y ecografía o tomografía para confirmar la apendicitis.', 1),
    (@IdServicio3, 'Paso', N'Cirugía laparoscópica', N'Bajo anestesia general se retira el apéndice por pequeñas incisiones. Dura alrededor de 45 minutos.', 2),
    (@IdServicio3, 'Paso', N'Hospitalización corta', N'En apendicitis no complicada, el alta suele ser en 24 a 48 horas.', 3),
    (@IdServicio3, 'Paso', N'Seguimiento', N'Control de heridas y retorno progresivo a la actividad.', 4),
    (@IdServicio3, 'Recuperacion', NULL, N'Caminar desde las primeras horas tras la cirugía.', 1),
    (@IdServicio3, 'Recuperacion', NULL, N'Dieta progresiva iniciando el mismo día o al siguiente.', 2),
    (@IdServicio3, 'Recuperacion', NULL, N'Retorno a estudios o trabajo de oficina en 1 a 2 semanas.', 3),
    (@IdServicio3, 'Recuperacion', NULL, N'Evitar esfuerzos físicos intensos durante 3 a 4 semanas.', 4),
    (@IdServicio3, 'Pregunta', N'¿La apendicitis siempre requiere cirugía?', N'En la gran mayoría de casos sí: la apendicectomía es el tratamiento estándar y el más seguro para evitar complicaciones.', 1),
    (@IdServicio3, 'Pregunta', N'¿Cuánto tiempo puedo esperar con dolor?', N'No se debe esperar. Si sospechas apendicitis, busca atención médica en las primeras horas: el riesgo de perforación aumenta con el tiempo.', 2),
    (@IdServicio3, 'Pregunta', N'¿Se puede vivir sin apéndice?', N'Sí, sin ninguna consecuencia para la salud ni la digestión.', 3);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.ServiciosContenido c JOIN dbo.ServiciosMedicos s ON s.Id = c.IdServicioMedico WHERE s.Slug = N'consulta-valoracion-quirurgica')
BEGIN
    DECLARE @IdServicio4 INT = (SELECT Id FROM dbo.ServiciosMedicos WHERE Slug = N'consulta-valoracion-quirurgica');
    INSERT INTO dbo.ServiciosContenido (IdServicioMedico, Tipo, Titulo, Texto, Orden) VALUES
    (@IdServicio4, 'Introduccion', NULL, N'La consulta de valoración es el primer paso antes de cualquier cirugía. En ella se revisan tus síntomas, antecedentes y exámenes para llegar a un diagnóstico preciso y decidir si realmente necesitas operarte.', 1),
    (@IdServicio4, 'Introduccion', NULL, N'También es el espacio ideal para pedir una segunda opinión: se explican con claridad las alternativas, los riesgos y los beneficios de cada opción, sin recomendar cirugías innecesarias.', 2),
    (@IdServicio4, 'Indicacion', NULL, N'Dolor abdominal recurrente o molestias digestivas sin diagnóstico claro.', 1),
    (@IdServicio4, 'Indicacion', NULL, N'Te detectaron cálculos en la vesícula, una hernia o un pólipo en una ecografía.', 2),
    (@IdServicio4, 'Indicacion', NULL, N'Te indicaron una cirugía y quieres una segunda opinión.', 3),
    (@IdServicio4, 'Indicacion', NULL, N'Tienes un bulto en la piel (lipoma, quiste) que crece o molesta.', 4),
    (@IdServicio4, 'Indicacion', NULL, N'Necesitas evaluación preoperatoria o seguimiento después de una cirugía.', 5),
    (@IdServicio4, 'Paso', N'Historia clínica', N'Conversamos sobre tus síntomas, antecedentes y medicamentos.', 1),
    (@IdServicio4, 'Paso', N'Examen físico', N'Evaluación dirigida según el motivo de consulta.', 2),
    (@IdServicio4, 'Paso', N'Revisión de exámenes', N'Se revisan ecografías, tomografías y análisis que traigas; si hace falta, se solicitan nuevos.', 3),
    (@IdServicio4, 'Paso', N'Plan de tratamiento', N'Recibes un diagnóstico y un plan claro: tratamiento médico, observación o cirugía programada.', 4),
    (@IdServicio4, 'Recuperacion', NULL, N'Trae tus exámenes previos (ecografías, tomografías, análisis) impresos o en digital.', 1),
    (@IdServicio4, 'Recuperacion', NULL, N'Lleva la lista de medicamentos que tomas habitualmente.', 2),
    (@IdServicio4, 'Recuperacion', NULL, N'Anota tus dudas para resolverlas todas en la consulta.', 3),
    (@IdServicio4, 'Pregunta', N'¿Necesito traer exámenes?', N'No es obligatorio, pero si los tienes ayudan a agilizar el diagnóstico. Si faltan, se solicitarán en la consulta.', 1),
    (@IdServicio4, 'Pregunta', N'¿Atienden con seguro?', N'Trabajamos con seguros aliados. Consulta tu cobertura al agendar tu cita.', 2),
    (@IdServicio4, 'Pregunta', N'¿Puedo pedir una segunda opinión?', N'Sí. Es una de las razones más frecuentes de consulta y es tu derecho como paciente antes de una cirugía.', 3);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.ServiciosContenido c JOIN dbo.ServiciosMedicos s ON s.Id = c.IdServicioMedico WHERE s.Slug = N'cirugia-lipomas-y-quistes')
BEGIN
    DECLARE @IdServicio5 INT = (SELECT Id FROM dbo.ServiciosMedicos WHERE Slug = N'cirugia-lipomas-y-quistes');
    INSERT INTO dbo.ServiciosContenido (IdServicioMedico, Tipo, Titulo, Texto, Orden) VALUES
    (@IdServicio5, 'Introduccion', NULL, N'Los lipomas (bultos de grasa bajo la piel), los quistes sebáceos y algunos lunares pueden retirarse con una cirugía menor, rápida y ambulatoria, usando anestesia local.', 1),
    (@IdServicio5, 'Introduccion', NULL, N'Se recomienda extirparlos cuando crecen, duelen, se inflaman, molestan con la ropa o por estética. La lesión se envía a análisis (patología) para confirmar que es benigna.', 2),
    (@IdServicio5, 'Indicacion', NULL, N'Bulto bajo la piel que ha crecido con el tiempo.', 1),
    (@IdServicio5, 'Indicacion', NULL, N'Quiste que se inflama, duele o drena material.', 2),
    (@IdServicio5, 'Indicacion', NULL, N'Lunar que cambia de forma, color o tamaño, o que sangra.', 3),
    (@IdServicio5, 'Indicacion', NULL, N'Lesiones que molestan con la ropa o por motivos estéticos.', 4),
    (@IdServicio5, 'Paso', N'Evaluación', N'Se examina la lesión y, si hace falta, se pide una ecografía de partes blandas.', 1),
    (@IdServicio5, 'Paso', N'Anestesia local', N'Se adormece solo la zona; el paciente está despierto y sin dolor.', 2),
    (@IdServicio5, 'Paso', N'Extirpación', N'Se retira la lesión completa y se cierra con puntos finos cuidando la cicatriz. Dura unos 30 minutos.', 3),
    (@IdServicio5, 'Paso', N'Patología y retiro de puntos', N'La muestra se analiza y los puntos se retiran en 7 a 10 días.', 4),
    (@IdServicio5, 'Recuperacion', NULL, N'Regreso a casa inmediatamente después del procedimiento.', 1),
    (@IdServicio5, 'Recuperacion', NULL, N'Actividades normales al día siguiente en la mayoría de casos.', 2),
    (@IdServicio5, 'Recuperacion', NULL, N'Curaciones sencillas en casa según indicaciones.', 3),
    (@IdServicio5, 'Recuperacion', NULL, N'Proteger la cicatriz del sol durante algunos meses.', 4),
    (@IdServicio5, 'Pregunta', N'¿Duele el procedimiento?', N'No. Con anestesia local solo se siente el pinchazo inicial; durante la cirugía no hay dolor.', 1),
    (@IdServicio5, 'Pregunta', N'¿Queda cicatriz?', N'Queda una cicatriz fina, que se procura orientar y cerrar con técnica estética para que sea lo menos visible posible.', 2),
    (@IdServicio5, 'Pregunta', N'¿Puede volver a salir?', N'Si la lesión se retira completa, la recurrencia es poco frecuente.', 3);
END
GO

-- 3.3 Relacionar testimonios de ejemplo con su procedimiento
UPDATE dbo.Testimonios SET IdServicioMedico = (SELECT Id FROM dbo.ServiciosMedicos WHERE Slug = N'cirugia-de-vesicula-laparoscopica') WHERE NombrePaciente LIKE N'Carlos Alberto%' AND IdServicioMedico IS NULL;
UPDATE dbo.Testimonios SET IdServicioMedico = (SELECT Id FROM dbo.ServiciosMedicos WHERE Slug = N'cirugia-de-hernia-inguinal-umbilical') WHERE NombrePaciente LIKE N'María Fernanda%' AND IdServicioMedico IS NULL;
UPDATE dbo.Testimonios SET IdServicioMedico = (SELECT Id FROM dbo.ServiciosMedicos WHERE Slug = N'consulta-valoracion-quirurgica') WHERE NombrePaciente LIKE N'Roberto Gómez%' AND IdServicioMedico IS NULL;
GO

-- 3.4 Categorías y artículos iniciales del blog (los mismos que muestra hoy la web)
IF NOT EXISTS (SELECT 1 FROM dbo.CategoriasBlog WHERE Slug = N'cirugia-de-vesicula') INSERT INTO dbo.CategoriasBlog (Nombre, Slug) VALUES (N'Cirugía de Vesícula', N'cirugia-de-vesicula');
IF NOT EXISTS (SELECT 1 FROM dbo.CategoriasBlog WHERE Slug = N'hernias') INSERT INTO dbo.CategoriasBlog (Nombre, Slug) VALUES (N'Hernias y Pared Abdominal', N'hernias');
IF NOT EXISTS (SELECT 1 FROM dbo.CategoriasBlog WHERE Slug = N'cuidados-postoperatorios') INSERT INTO dbo.CategoriasBlog (Nombre, Slug) VALUES (N'Cuidados Postoperatorios', N'cuidados-postoperatorios');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.ArticulosBlog WHERE Slug = N'como-saber-si-tengo-calculos-en-la-vesicula-sintomas')
INSERT INTO dbo.ArticulosBlog (Titulo, Slug, Resumen, ContenidoHtml, ImagenPortadaUrl, IdCategoria, IdServicioMedico, Autor, TiempoLecturaMinutos, FechaPublicacion, PublicadoEnWeb)
VALUES (N'¿Cómo saber si tengo cálculos en la vesícula? 5 síntomas clave', N'como-saber-si-tengo-calculos-en-la-vesicula-sintomas', N'El dolor abdominal después de comer grasas no siempre es indigestión común. Conoce las señales de alarma de la colelitiasis y por qué la cirugía temprana evita emergencias.', N'<p class="lead font-medium text-slate-700 dark:text-slate-300 mb-6">
        Los cálculos biliares o piedras en la vesícula (colelitiasis) son una de las afecciones digestivas más frecuentes en nuestro país. Muchas veces, los pacientes confunden los síntomas iniciales con una simple gastritis o indigestión pasajera, retrasando una evaluación médica especializada que podría prevenir complicaciones graves como pancreatitis o colecistitis aguda.
      </p>

      <h3 class="text-xl font-bold text-slate-900 dark:text-white mt-8 mb-4">1. Dolor en el lado derecho del abdomen (Cólico Biliar)</h3>
      <p class="mb-4">
        El síntoma más característico es un dolor intenso, punzante u opresivo en la parte superior derecha del abdomen, justo debajo de las costillas. Este dolor suele desencadenarse entre 30 minutos y 2 horas después de ingerir comidas copiosas o ricas en grasas (frituras, lácteos enteros, carnes rojas). En ocasiones, el dolor se irradia hacia la espalda o el hombro derecho.
      </p>

      <h3 class="text-xl font-bold text-slate-900 dark:text-white mt-8 mb-4">2. Náuseas persistentes y vómitos biliosos</h3>
      <p class="mb-4">
        Cuando un cálculo obstruye el conducto cístico, la bilis no puede fluir correctamente para digerir los alimentos. Esto genera una sensación de náusea profunda y vómitos de color amarillento o verdoso que no alivian completamente el malestar.
      </p>

      <h3 class="text-xl font-bold text-slate-900 dark:text-white mt-8 mb-4">3. Intolerancia a las grasas y digestión lenta</h3>
      <p class="mb-4">
        Si notas que cada vez que comes huevo, frituras o aderezos experimentas hinchazón abdominal, gases excesivos y pesadez que dura varias horas, es probable que tu vesícula no esté contrayéndose de forma adecuada debido a la presencia de cálculos o lodo biliar.
      </p>

      <h3 class="text-xl font-bold text-slate-900 dark:text-white mt-8 mb-4">4. Coloración amarillenta en ojos o piel (Ictericia)</h3>
      <p class="mb-4">
        <strong>⚠️ Señal de alerta médica:</strong> Si el color blanco de tus ojos adquiere un tono amarillo, la orina se vuelve oscura (color té) y las heces son muy claras, significa que un cálculo ha migrado y está bloqueando el conducto biliar principal (colédoco). Esta es una urgencia quirúrgica que requiere atención especializada inmediata.
      </p>

      <h3 class="text-xl font-bold text-slate-900 dark:text-white mt-8 mb-4">5. Fiebre y escalofríos asociados a dolor abdominal</h3>
      <p class="mb-4">
        La combinación de fiebre, escalofríos y dolor en el hipocondrio derecho indica que la vesícula se ha infectado (colecistitis aguda). No recurras al automedicamiento con analgésicos fuertes, ya que pueden enmascarar el cuadro y llevar a una perforación vesicular.
      </p>

      <div class="bg-medical-50 dark:bg-navy-900/80 p-6 rounded-2xl border-l-4 border-medical-500 my-8">
        <h4 class="font-bold text-slate-900 dark:text-white text-base mb-2">Recomendación del Dr. Juan Carlos Marcos Enriquez:</h4>
        <p class="text-sm text-slate-600 dark:text-slate-300">
          "El tratamiento de elección y definitivo para los cálculos vesiculares es la <strong>Colecistectomía Laparoscópica</strong>. Realizada a tiempo y de forma programada, es una intervención ultra-segura de mínima invasión, con micro-incisiones de 5 a 10 mm que permiten el alta médica en tan solo 24 horas y una rápida reincorporación a tu rutina laboral."
        </p>
      </div>', N'/WhatsApp Image 2026-07-02 at 12.36.06 PM (2).jpeg',
    (SELECT Id FROM dbo.CategoriasBlog WHERE Slug = N'cirugia-de-vesicula'),
    (SELECT Id FROM dbo.ServiciosMedicos WHERE Slug = N'cirugia-de-vesicula-laparoscopica'),
    N'Dr. Juan Carlos Marcos Enriquez', 4, '2026-07-02', 1);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.ArticulosBlog WHERE Slug = N'hernia-inguinal-o-abdominal-por-que-nunca-se-curan-solas')
INSERT INTO dbo.ArticulosBlog (Titulo, Slug, Resumen, ContenidoHtml, ImagenPortadaUrl, IdCategoria, IdServicioMedico, Autor, TiempoLecturaMinutos, FechaPublicacion, PublicadoEnWeb)
VALUES (N'Hernia Inguinal o Abdominal: ¿Por qué nunca se curan solas y cuándo operar?', N'hernia-inguinal-o-abdominal-por-que-nunca-se-curan-solas', N'Un bulto en la ingle o el ombligo es señal de una debilidad en la pared muscular. Descubre los riesgos del estrangulamiento y las ventajas de la plastía con malla sin tensión.', N'<p class="lead font-medium text-slate-700 dark:text-slate-300 mb-6">
        Una hernia ocurre cuando un órgano interno (generalmente una porción del intestino) empuja a través de un punto débil o un desgarro en el músculo o tejido que lo contiene. El mito más peligroso es pensar que con fajas, reposo o cremas el bulto desaparecerá. En cirugía general, es una máxima médica: <strong>una hernia jamás se cura sola; con el tiempo solo crece y aumenta su riesgo.</strong>
      </p>

      <h3 class="text-xl font-bold text-slate-900 dark:text-white mt-8 mb-4">¿Cuáles son los síntomas más comunes?</h3>
      <ul class="list-disc pl-6 space-y-2 mb-6 text-slate-600 dark:text-slate-300">
        <li>Aparición de un bulto o protuberancia visible en la ingle (hernia inguinal), el ombligo (hernia umbilical) o en una cicatriz de cirugía previa (hernia incisional o eventración).</li>
        <li>Sensación de ardor, dolor o pesadez en la zona, especialmente al toser, levantar objetos pesados, agacharse o permanecer de pie muchas horas.</li>
        <li>El bulto desaparece cuando te acuestas boca arriba (hernia reductible), pero vuelve a salir al ponerte de pie.</li>
      </ul>

      <h3 class="text-xl font-bold text-slate-900 dark:text-white mt-8 mb-4">El gran peligro: Hernia Incarcerada o Estrangulada</h3>
      <p class="mb-4">
        Si el intestino queda atrapado dentro del orificio de la hernia y no puede regresar a la cavidad abdominal, el flujo sanguíneo se corta (estrangulamiento). Esto provoca una necrosis del tejido intestinal en cuestión de horas, convirtiendo una cirugía rutinaria en una emergencia vital con riesgo de peritonitis.
      </p>

      <div class="bg-medical-50 dark:bg-navy-900/80 p-6 rounded-2xl border-l-4 border-medical-500 my-8">
        <h4 class="font-bold text-slate-900 dark:text-white text-base mb-2">Filosofía de Mínima Invasión (Plastía con Malla):</h4>
        <p class="text-sm text-slate-600 dark:text-slate-300">
          "En nuestra práctica quirúrgica empleamos técnicas laparoscópicas avanzadas con colocación de mallas protésicas biocompatibles libres de tensión. Esto refuerza la pared muscular desde el interior, reduciendo la tasa de reaparición a casi 0% y garantizando que el paciente camine sin dolor el mismo día de la intervención."
        </p>
      </div>', N'/WhatsApp Image 2026-07-02 at 12.36.06 PM (4).jpeg',
    (SELECT Id FROM dbo.CategoriasBlog WHERE Slug = N'hernias'),
    (SELECT Id FROM dbo.ServiciosMedicos WHERE Slug = N'cirugia-de-hernia-inguinal-umbilical'),
    N'Dr. Juan Carlos Marcos Enriquez', 5, '2026-06-28', 1);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.ArticulosBlog WHERE Slug = N'dieta-y-cuidados-postoperatorios-que-comer-tras-cirugia-digestiva')
INSERT INTO dbo.ArticulosBlog (Titulo, Slug, Resumen, ContenidoHtml, ImagenPortadaUrl, IdCategoria, IdServicioMedico, Autor, TiempoLecturaMinutos, FechaPublicacion, PublicadoEnWeb)
VALUES (N'Dieta y cuidados postoperatorios: ¿Qué comer tras una cirugía digestiva?', N'dieta-y-cuidados-postoperatorios-que-comer-tras-cirugia-digestiva', N'Guía práctica para una recuperación óptima en casa. Alimentos permitidos, hidratación y cómo cuidar tus cicatrices laparoscópicas durante la primera semana.', N'<p class="lead font-medium text-slate-700 dark:text-slate-300 mb-6">
        El éxito de una cirugía no termina al salir del quirófano. El postoperatorio en el hogar es una etapa fundamental donde una correcta alimentación y cuidados de herida aceleran la cicatrización tisular y evitan molestias digestivas.
      </p>

      <h3 class="text-xl font-bold text-slate-900 dark:text-white mt-8 mb-4">Fase 1: Primeras 48 horas (Dieta Blanda y Baja en Grasas)</h3>
      <p class="mb-4">
        Tras una cirugía digestiva por laparoscopia, el intestino necesita readaptarse gradualmente:
      </p>
      <ul class="list-disc pl-6 space-y-2 mb-6 text-slate-600 dark:text-slate-300">
        <li><strong>Alimentos recomendados:</strong> Caldos desgrasados, sopas de verduras licuadas, compota de manzana, gelatina, pollo sancochado o a la plancha desmenuzado, arroz blanco y tostadas.</li>
        <li><strong>Hidratación:</strong> Bebe agua, infusiones tibias (manzanilla o anís) en pequeños sorbos frecuentes. Evita usar cañitas o pajitas, ya que introducen aire al estómago.</li>
        <li><strong>Prohibido los primeros días:</strong> Frituras, lácteos enteros (leche, queso, mantequilla), carnes rojas, gaseosas, alcohol, legumbres y condimentos picantes.</li>
      </ul>

      <h3 class="text-xl font-bold text-slate-900 dark:text-white mt-8 mb-4">Cuidado de las micro-incisiones laparoscópicas</h3>
      <p class="mb-4">
        Al ser incisiones de apenas unos milímetros, su cuidado es muy sencillo: puedes ducharte con agua y jabón neutro dejando correr el agua suavemente sobre los apósitos o cicatrices sin frotar. Seca la zona con una toalla limpia dando pequeños toques. No apliques ungüentos ni alcohol sobre las heridas cerradas.
      </p>

      <div class="bg-emerald-500/10 p-6 rounded-2xl border border-emerald-500/20 my-8">
        <h4 class="font-bold text-emerald-700 dark:text-emerald-400 text-base mb-2">⚡ Ventaja de nuestra Técnica Quirúrgica:</h4>
        <p class="text-sm text-slate-600 dark:text-slate-300">
          "Gracias a nuestro protocolo de analgesia multimodal y suturas estéticas intradérmicas, más del 95% de nuestros pacientes no requieren retirar puntos postoperatorios y reportan una reincorporación confortable a sus actividades cotidianas al quinto día."
        </p>
      </div>', N'/WhatsApp Image 2026-07-02 at 12.36.06 PM (3).jpeg',
    (SELECT Id FROM dbo.CategoriasBlog WHERE Slug = N'cuidados-postoperatorios'),
    NULL,
    N'Dr. Juan Carlos Marcos Enriquez', 4, '2026-06-15', 1);
GO

