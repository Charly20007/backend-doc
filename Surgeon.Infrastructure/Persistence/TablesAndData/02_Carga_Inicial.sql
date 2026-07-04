                    -- =========================================================================
-- SCRIPT DE CARGA INICIAL DE CATÁLOGOS - CIRUJANO GENERAL
-- =========================================================================

-- 1. Roles del Sistema
INSERT INTO dbo.Roles (Nombre)
VALUES ('Administrador'), ('Cirujano'), ('Secretaria'), ('Paciente');
GO

-- 2. Estados de Cita (Con códigos de color compatibles con Tailwind CSS en CMS)
INSERT INTO dbo.EstadosCita (Nombre, ColorHex)
VALUES 
('Pendiente', '#F59E0B'),  -- Amber-500
('Confirmada', '#0284C7'), -- Cerulean-600
('Atendida', '#3B82F6'),   -- Blue-500
('Cancelada', '#EF4444');  -- Red-500
GO

-- 3. Especialidades Médicas
INSERT INTO dbo.Especialidades (Nombre, Descripcion, Icono)
VALUES 
('Cirugía General', 'Diagnóstico y tratamiento quirúrgico del tracto gastrointestinal y sistema endocrino.', 'Stethoscope'),
('Cirugía Laparoscópica', 'Técnicas mínimamente invasivas para cirugías con rápida recuperación y mínimas cicatrices.', 'Activity'),
('Cirugía de Pared Abdominal', 'Tratamiento especializado de hernias inguinales, umbilicales y eventraciones complejas.', 'Shield'),
('Cirugía Menor Ambulatoria', 'Extirpación de lesiones en piel y tejidos blandos (lipomas, quistes, verrugas) sin hospitalización.', 'Scissors');
GO

-- 4. Servicios Médicos y Procedimientos (Con Slug para SEO)
INSERT INTO dbo.ServiciosMedicos (Nombre, Slug, Resumen, DescripcionDetallada, TiempoEstimadoMinutos, PrecioReferencial, Icono, IdEspecialidad, DestacadoEnWeb)
VALUES 
('Colecistectomía Laparoscópica (Cirugía de Vesícula)', 'cirugia-de-vesicula-laparoscopica', 'Extirpación de la vesícula biliar por mínima invasión. Rápida recuperación en 24 a 48 horas.', 'La cirugía laparoscópica de vesícula es el estándar de oro para tratar cálculos biliares. Se realiza a través de pequeñas incisiones, lo que reduce drásticamente el dolor postoperatorio y permite reintegrarse a las actividades cotidianas rápidamente.', 60, 3500.00, 'Activity', 2, 1),
('Reparación de Hernia Inguinal o Umbilical', 'cirugia-de-hernia-inguinal-umbilical', 'Corrección de hernias con colocación de malla biocompatible para evitar recurrencias.', 'Tratamiento definitivo para hernias abdominales o inguinales mediante cirugía laparoscópica o abierta técnica sin tensión con malla de alta calidad.', 60, 3000.00, 'Shield', 3, 1),
('Apendicectomía Laparoscópica (Cirugía de Apéndice)', 'cirugia-de-apendice-laparoscopica', 'Tratamiento seguro y oportuno para la apendicitis aguda con cicatrices casi invisibles.', 'Procedimiento de urgencia o programado realizado por vía laparoscópica con estancia hospitalaria corta y mínima incomodidad.', 45, 3200.00, 'Activity', 2, 1),
('Consulta de Valoración Quirúrgica', 'consulta-valoracion-quirurgica', 'Evaluación médica integral para diagnóstico, revisión de estudios y planificación de cirugía.', 'En esta consulta analizaremos sus síntomas, revisaremos análisis de laboratorio o ecografías y determinaremos el mejor enfoque terapéutico o quirúrgico para su caso.', 30, 200.00, 'Stethoscope', 1, 1),
('Cirugía de Lesiones en Tejidos Blandos (Lipomas y Quistes)', 'cirugia-lipomas-y-quistes', 'Extirpación ambulatoria con anestesia local y excelentes resultados estéticos.', 'Procedimiento rápido y seguro en el consultorio o sala de procedimientos menores para extirpar quistes sebáceos, lipomas o lunares sospechosos.', 30, 500.00, 'Scissors', 4, 1);
GO

-- 5. Horarios de Atención Regulares (Lunes a Viernes de 9 AM a 1 PM y 4 PM a 8 PM)
INSERT INTO dbo.HorariosAtencion (DiaSemana, HoraInicio, HoraFin, EsActivo)
VALUES 
(1, '09:00:00', '13:00:00', 1), (1, '16:00:00', '20:00:00', 1), -- Lunes
(2, '09:00:00', '13:00:00', 1), (2, '16:00:00', '20:00:00', 1), -- Martes
(3, '09:00:00', '13:00:00', 1), (3, '16:00:00', '20:00:00', 1), -- Miércoles
(4, '09:00:00', '13:00:00', 1), (4, '16:00:00', '20:00:00', 1), -- Jueves
(5, '09:00:00', '13:00:00', 1), (5, '16:00:00', '20:00:00', 1), -- Viernes
(6, '09:00:00', '13:00:00', 1);                              -- Sábado mañana
GO

-- 6. Categorías para Blog Médico
INSERT INTO dbo.CategoriasBlog (Nombre, Slug)
VALUES 
('Consejos Preoperatorios', 'consejos-preoperatorios'),
('Recuperación y Cuidados Postoperatorios', 'recuperacion-y-cuidados'),
('Mitos y Realidades en Cirugía', 'mitos-y-realidades'),
('Nutrición Saludable', 'nutricion-saludable');
GO

-- 7. Testimonios de Ejemplo para la Web
INSERT INTO dbo.Testimonios (NombrePaciente, Procedimiento, CalificacionEstrellas, Comentario, AprobadoParaWeb)
VALUES 
('Carlos Alberto Ramos', 'Cirugía de Vesícula Laparoscópica', 5, 'Excelente atención del doctor. Me operé el martes y para el jueves ya estaba caminando sin dolor en casa. Muy agradecido por su profesionalismo y amabilidad.', 1),
('María Fernanda López', 'Reparación de Hernia Umbilical', 5, 'Me explicó todo el procedimiento con mucha paciencia en la consulta de valoración. La cirugía fue un éxito y la cicatriz apenas se nota. 100% recomendado.', 1),
('Roberto Gómez', 'Consulta de Valoración Quirúrgica', 5, 'Muy puntual y certero en su diagnóstico. Evitó una cirugía innecesaria con el tratamiento médico adecuado.', 1);
GO
