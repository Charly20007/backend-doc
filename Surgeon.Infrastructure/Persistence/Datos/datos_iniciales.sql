-- =========================================================================
-- DATOS INICIALES (PostgreSQL) - CIRUJANO GENERAL
-- -------------------------------------------------------------------------
-- La estructura de tablas la crea la API al arrancar (migraciones de EF Core).
-- Este script carga el contenido inicial en una base NUEVA y VACÍA:
--   psql -U surgeon -d surgeon -f datos_iniciales.sql
-- No incluye pacientes, citas, mensajes ni usuarios: el administrador lo crea
-- la API con la sección "AdminInicial" de la configuración.
-- Generado desde la base actual con pg_dump.
-- =========================================================================




-- Data for Name: categorias_blog; Type: TABLE DATA; Schema: public; Owner: -

INSERT INTO public.categorias_blog (id, nombre, slug, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (1, 'Consejos Preoperatorios', 'consejos-preoperatorios', 'UsrSurgeon', '2026-11-21 18:48:14.14', '::1', false);
INSERT INTO public.categorias_blog (id, nombre, slug, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (2, 'Recuperación y Cuidados Postoperatorios', 'recuperacion-y-cuidados', 'UsrSurgeon', '2026-11-21 18:48:14.14', '::1', false);
INSERT INTO public.categorias_blog (id, nombre, slug, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (3, 'Mitos y Realidades en Cirugía', 'mitos-y-realidades', 'UsrSurgeon', '2026-11-21 18:48:14.14', '::1', false);
INSERT INTO public.categorias_blog (id, nombre, slug, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (4, 'Nutrición Saludable', 'nutricion-saludable', 'UsrSurgeon', '2026-11-21 18:48:14.14', '::1', false);
INSERT INTO public.categorias_blog (id, nombre, slug, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (5, 'Cirugía de Vesícula', 'cirugia-de-vesicula', 'UsrSurgeon', '2026-11-21 18:48:14.453', '::1', false);
INSERT INTO public.categorias_blog (id, nombre, slug, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (6, 'Hernias y Pared Abdominal', 'hernias', 'UsrSurgeon', '2026-11-21 18:48:14.453', '::1', false);
INSERT INTO public.categorias_blog (id, nombre, slug, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (7, 'Cuidados Postoperatorios', 'cuidados-postoperatorios', 'UsrSurgeon', '2026-11-21 18:48:14.453', '::1', false);


-- Data for Name: especialidades; Type: TABLE DATA; Schema: public; Owner: -

INSERT INTO public.especialidades (id, nombre, descripcion, icono, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (1, 'Cirugía General', 'Diagnóstico y tratamiento quirúrgico del tracto gastrointestinal y sistema endocrino.', 'Stethoscope', 'UsrSurgeon', '2026-11-21 18:48:14.127', '::1', false);
INSERT INTO public.especialidades (id, nombre, descripcion, icono, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (2, 'Cirugía Laparoscópica', 'Técnicas mínimamente invasivas para cirugías con rápida recuperación y mínimas cicatrices.', 'Activity', 'UsrSurgeon', '2026-11-21 18:48:14.127', '::1', false);
INSERT INTO public.especialidades (id, nombre, descripcion, icono, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (3, 'Cirugía de Pared Abdominal', 'Tratamiento especializado de hernias inguinales, umbilicales y eventraciones complejas.', 'Shield', 'UsrSurgeon', '2026-11-21 18:48:14.127', '::1', false);
INSERT INTO public.especialidades (id, nombre, descripcion, icono, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (4, 'Cirugía Menor Ambulatoria', 'Extirpación de lesiones en piel y tejidos blandos (lipomas, quistes, verrugas) sin hospitalización.', 'Scissors', 'UsrSurgeon', '2026-11-21 18:48:14.127', '::1', false);


-- Data for Name: servicios_medicos; Type: TABLE DATA; Schema: public; Owner: -

INSERT INTO public.servicios_medicos (id, nombre, nombre_corto, slug, resumen, descripcion_detallada, tiempo_estimado_minutos, precio_referencial, icono, id_especialidad, destacado_en_web, orden, condicion_tratada, titulo_indicaciones, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (1, 'Colecistectomía Laparoscópica (Cirugía de Vesícula)', 'Cirugía de Vesícula', 'cirugia-de-vesicula-laparoscopica', 'Extirpación de la vesícula biliar por mínima invasión. Rápida recuperación en 24 a 48 horas sin cicatrices visibles.', 'La cirugía laparoscópica de vesícula es el estándar de oro para tratar cálculos biliares. Se realiza a través de pequeñas incisiones, lo que reduce drásticamente el dolor postoperatorio y permite reintegrarse a las actividades cotidianas rápidamente.', 60, 3500.00, 'Activity', 2, true, 1, 'Colelitiasis (cálculos en la vesícula)', '¿Cuándo se recomienda operar la vesícula?', 'Cirugía de Vesícula Laparoscópica en Lima', 'Operación de vesícula por laparoscopía en Lima: síntomas, cómo es la cirugía, recuperación y precio. Agenda tu evaluación en San Isidro.', NULL, true, 'admin', '2026-11-21 19:00:31.197', '0.0.0.1', false);
INSERT INTO public.servicios_medicos (id, nombre, nombre_corto, slug, resumen, descripcion_detallada, tiempo_estimado_minutos, precio_referencial, icono, id_especialidad, destacado_en_web, orden, condicion_tratada, titulo_indicaciones, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (2, 'Reparación de Hernia Inguinal o Umbilical', 'Cirugía de Hernia', 'cirugia-de-hernia-inguinal-umbilical', 'Corrección quirúrgica con colocación de malla biocompatible de alta calidad para evitar recurrencias y aliviar el dolor al instante.', 'Tratamiento definitivo para hernias abdominales o inguinales mediante cirugía laparoscópica o abierta técnica sin tensión con malla de alta calidad.', 60, 3000.00, 'Shield', 3, true, 2, 'Hernia de pared abdominal', 'Síntomas de una hernia', 'Cirugía de Hernia Inguinal y Umbilical en Lima', 'Reparación de hernia inguinal, umbilical o incisional con malla y técnica laparoscópica o abierta. Síntomas, recuperación y precio referencial en San Isidro.', NULL, true, 'UsrSurgeon', '2026-11-21 18:48:14.133', '::1', false);
INSERT INTO public.servicios_medicos (id, nombre, nombre_corto, slug, resumen, descripcion_detallada, tiempo_estimado_minutos, precio_referencial, icono, id_especialidad, destacado_en_web, orden, condicion_tratada, titulo_indicaciones, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (3, 'Apendicectomía Laparoscópica (Cirugía de Apéndice)', 'Cirugía de Apéndice', 'cirugia-de-apendice-laparoscopica', 'Tratamiento seguro y oportuno para la apendicitis aguda con incisiones milimétricas, mínima estancia y recuperación inmediata.', 'Procedimiento de urgencia o programado realizado por vía laparoscópica con estancia hospitalaria corta y mínima incomodidad.', 45, 3200.00, 'Activity', 2, true, 3, 'Apendicitis aguda', 'Síntomas de apendicitis', 'Cirugía de Apendicitis Laparoscópica en Lima', 'Apendicectomía laparoscópica de urgencia: síntomas de apendicitis, cómo es la operación y recuperación. Atención de emergencias quirúrgicas en Lima.', NULL, true, 'UsrSurgeon', '2026-11-21 18:48:14.133', '::1', false);
INSERT INTO public.servicios_medicos (id, nombre, nombre_corto, slug, resumen, descripcion_detallada, tiempo_estimado_minutos, precio_referencial, icono, id_especialidad, destacado_en_web, orden, condicion_tratada, titulo_indicaciones, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (4, 'Consulta de Valoración Quirúrgica', 'Consulta de Valoración', 'consulta-valoracion-quirurgica', 'Evaluación médica integral para diagnóstico preciso, revisión ecográfica/tomográfica y planificación personalizada de cirugía.', 'En esta consulta analizaremos sus síntomas, revisaremos análisis de laboratorio o ecografías y determinaremos el mejor enfoque terapéutico o quirúrgico para su caso.', 30, 200.00, 'Stethoscope', 1, true, 4, NULL, '¿Cuándo pedir una consulta?', 'Consulta con Cirujano General en San Isidro', 'Consulta de valoración quirúrgica y segunda opinión con cirujano general: revisión de ecografías y tomografías, diagnóstico y plan de tratamiento.', NULL, true, 'UsrSurgeon', '2026-11-21 18:48:14.133', '::1', false);
INSERT INTO public.servicios_medicos (id, nombre, nombre_corto, slug, resumen, descripcion_detallada, tiempo_estimado_minutos, precio_referencial, icono, id_especialidad, destacado_en_web, orden, condicion_tratada, titulo_indicaciones, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (5, 'Cirugía de Lesiones en Tejidos Blandos (Lipomas y Quistes)', 'Lipomas y Quistes', 'cirugia-lipomas-y-quistes', 'Procedimiento ambulatorio indoloro con anestesia local para lesiones en tejidos blandos con el mejor resultado estético.', 'Procedimiento rápido y seguro en el consultorio o sala de procedimientos menores para extirpar quistes sebáceos, lipomas o lunares sospechosos.', 30, 500.00, 'Scissors', 4, true, 5, 'Lipomas y quistes de piel', '¿Cuándo conviene retirarlos?', 'Extirpación de Lipomas y Quistes Sebáceos en Lima', 'Retiro de lipomas, quistes sebáceos y lunares con anestesia local, de forma ambulatoria y con cuidado estético. Agenda tu evaluación en San Isidro.', NULL, true, 'admin', '2026-11-21 19:11:09.893', '0.0.0.1', false);


-- Data for Name: articulos_blog; Type: TABLE DATA; Schema: public; Owner: -

INSERT INTO public.articulos_blog (id, titulo, slug, resumen, contenido_html, imagen_portada_url, id_categoria, id_servicio_medico, autor, tiempo_lectura_minutos, fecha_publicacion, publicado_en_web, vistas, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (1, '¿Cómo saber si tengo cálculos en la vesícula? 5 síntomas clave', 'como-saber-si-tengo-calculos-en-la-vesicula-sintomas', 'El dolor abdominal después de comer grasas no siempre es indigestión común. Conoce las señales de alarma de la colelitiasis y por qué la cirugía temprana evita emergencias.', '<p class="lead font-medium text-slate-700 dark:text-slate-300 mb-6">
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
      </div>', '/WhatsApp Image 2026-07-02 at 12.36.06 PM (2).jpeg', 5, 1, 'Dr. Juan Carlos Marcos Enriquez', 4, '2026-07-02 00:00:00', true, 0, NULL, NULL, NULL, true, 'UsrSurgeon', '2026-11-21 18:48:14.46', '::1', false);
INSERT INTO public.articulos_blog (id, titulo, slug, resumen, contenido_html, imagen_portada_url, id_categoria, id_servicio_medico, autor, tiempo_lectura_minutos, fecha_publicacion, publicado_en_web, vistas, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (2, 'Hernia Inguinal o Abdominal: ¿Por qué nunca se curan solas y cuándo operar?', 'hernia-inguinal-o-abdominal-por-que-nunca-se-curan-solas', 'Un bulto en la ingle o el ombligo es señal de una debilidad en la pared muscular. Descubre los riesgos del estrangulamiento y las ventajas de la plastía con malla sin tensión.', '<p class="lead font-medium text-slate-700 dark:text-slate-300 mb-6">
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
      </div>', '/WhatsApp Image 2026-07-02 at 12.36.06 PM (4).jpeg', 6, 2, 'Dr. Juan Carlos Marcos Enriquez', 5, '2026-06-28 00:00:00', true, 0, NULL, NULL, NULL, true, 'UsrSurgeon', '2026-11-21 18:48:14.47', '::1', false);
INSERT INTO public.articulos_blog (id, titulo, slug, resumen, contenido_html, imagen_portada_url, id_categoria, id_servicio_medico, autor, tiempo_lectura_minutos, fecha_publicacion, publicado_en_web, vistas, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (3, 'Dieta y cuidados postoperatorios: ¿Qué comer tras una cirugía digestiva?', 'dieta-y-cuidados-postoperatorios-que-comer-tras-cirugia-digestiva', 'Guía práctica para una recuperación óptima en casa. Alimentos permitidos, hidratación y cómo cuidar tus cicatrices laparoscópicas durante la primera semana.', '<p class="lead font-medium text-slate-700 dark:text-slate-300 mb-6">
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
      </div>', '/WhatsApp Image 2026-07-02 at 12.36.06 PM (3).jpeg', 7, NULL, 'Dr. Juan Carlos Marcos Enriquez', 4, '2026-06-15 00:00:00', true, 0, NULL, NULL, NULL, true, 'UsrSurgeon', '2026-11-21 18:48:14.477', '::1', false);


-- Data for Name: configuracion_sitio; Type: TABLE DATA; Schema: public; Owner: -

INSERT INTO public.configuracion_sitio (id, nombre_doctor, nombre_corto, especialidad, cmp, rne, foto_doctor_url, seguros_aliados, telefono_principal, telefono_secundario, whats_app, correo, direccion, distrito, ciudad, region, pais, google_maps_url, descripcion_pie, mostrar_emergencias, texto_emergencias, facebook_url, instagram_url, tik_tok_url, you_tube_url, linked_in_url, site_url, meta_titulo_por_defecto, meta_descripcion_por_defecto, imagen_og_por_defecto_url, google_site_verification, google_analytics_id, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (1, 'Dr. Juan Marcos', 'Dr. Juan Marcos', 'Especialista en Cirugía General y del Aparato Digestivo', '45892', '23104', '/WhatsApp Image 2026-07-02 at 12.36.06 PM (1).jpeg', 'Pacífico, Rímac, Mapfre y Sanitas', '+5119998887', '+51987654321', NULL, 'consultas@cirujanogeneral.pe', 'Av. Javier Prado Este 1234, Consultorio 502', 'San Isidro', 'Lima', 'Lima', 'PE', NULL, 'Cirugía general y del aparato digestivo con técnicas de mínima invasión.', false, 'Emergencias quirúrgicas: atención las 24 horas', NULL, NULL, NULL, NULL, NULL, 'https://cirujanogeneral.pe', 'Dr. Juan Carlos Marcos Enriquez | Cirugía General & Laparoscópica', 'Especialista en Cirugía General y del Aparato Digestivo por Mínima Invasión. Cirugía de vesícula, hernias, apendicitis y laparoscopía avanzada en Lima, Perú.', '/WhatsApp Image 2026-07-02 at 12.36.06 PM (2).jpeg', NULL, NULL, 'admin', '2026-11-21 20:13:27.703', '0.0.0.1', false);


-- Data for Name: estados_cita; Type: TABLE DATA; Schema: public; Owner: -

INSERT INTO public.estados_cita (id, nombre, color_hex, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (1, 'Pendiente', '#F59E0B', 'UsrSurgeon', '2026-11-21 18:48:14.123', '::1', false);
INSERT INTO public.estados_cita (id, nombre, color_hex, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (2, 'Confirmada', '#0284C7', 'UsrSurgeon', '2026-11-21 18:48:14.123', '::1', false);
INSERT INTO public.estados_cita (id, nombre, color_hex, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (3, 'Atendida', '#3B82F6', 'UsrSurgeon', '2026-11-21 18:48:14.123', '::1', false);
INSERT INTO public.estados_cita (id, nombre, color_hex, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (4, 'Cancelada', '#EF4444', 'UsrSurgeon', '2026-11-21 18:48:14.123', '::1', false);


-- Data for Name: horarios_atencion; Type: TABLE DATA; Schema: public; Owner: -

INSERT INTO public.horarios_atencion (id, dia_semana, hora_inicio, hora_fin, es_activo, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (12, 1, '09:00:00', '13:00:00', true, 'admin', '2026-11-21 20:13:27.71', '0.0.0.1', false);
INSERT INTO public.horarios_atencion (id, dia_semana, hora_inicio, hora_fin, es_activo, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (13, 1, '16:00:00', '20:00:00', true, 'admin', '2026-11-21 20:13:27.72', '0.0.0.1', false);
INSERT INTO public.horarios_atencion (id, dia_semana, hora_inicio, hora_fin, es_activo, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (14, 2, '09:00:00', '13:00:00', true, 'admin', '2026-11-21 20:13:27.72', '0.0.0.1', false);
INSERT INTO public.horarios_atencion (id, dia_semana, hora_inicio, hora_fin, es_activo, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (15, 2, '16:00:00', '20:00:00', true, 'admin', '2026-11-21 20:13:27.72', '0.0.0.1', false);
INSERT INTO public.horarios_atencion (id, dia_semana, hora_inicio, hora_fin, es_activo, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (16, 3, '09:00:00', '13:00:00', true, 'admin', '2026-11-21 20:13:27.72', '0.0.0.1', false);
INSERT INTO public.horarios_atencion (id, dia_semana, hora_inicio, hora_fin, es_activo, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (17, 3, '16:00:00', '20:00:00', true, 'admin', '2026-11-21 20:13:27.72', '0.0.0.1', false);
INSERT INTO public.horarios_atencion (id, dia_semana, hora_inicio, hora_fin, es_activo, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (18, 4, '09:00:00', '13:00:00', true, 'admin', '2026-11-21 20:13:27.72', '0.0.0.1', false);
INSERT INTO public.horarios_atencion (id, dia_semana, hora_inicio, hora_fin, es_activo, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (19, 4, '16:00:00', '20:00:00', true, 'admin', '2026-11-21 20:13:27.72', '0.0.0.1', false);
INSERT INTO public.horarios_atencion (id, dia_semana, hora_inicio, hora_fin, es_activo, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (20, 5, '09:00:00', '13:00:00', true, 'admin', '2026-11-21 20:13:27.72', '0.0.0.1', false);
INSERT INTO public.horarios_atencion (id, dia_semana, hora_inicio, hora_fin, es_activo, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (21, 5, '16:00:00', '20:00:00', true, 'admin', '2026-11-21 20:13:27.72', '0.0.0.1', false);
INSERT INTO public.horarios_atencion (id, dia_semana, hora_inicio, hora_fin, es_activo, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (22, 6, '09:00:00', '13:00:00', true, 'admin', '2026-11-21 20:13:27.72', '0.0.0.1', false);


-- Data for Name: paginas_legales; Type: TABLE DATA; Schema: public; Owner: -

INSERT INTO public.paginas_legales (id, clave, ruta, titulo, contenido_html, publicada, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (1, 'privacidad', '/privacidad', 'Política de privacidad y protección de datos personales', '<p>Esta política explica cómo se tratan los datos personales que usted nos proporciona a través de este sitio web, en cumplimiento de la Ley N.° 29733, Ley de Protección de Datos Personales, y su Reglamento.</p>
<h2>¿Quién es el responsable de sus datos?</h2>
<p>El responsable del tratamiento es el médico titular del consultorio, cuyos datos de contacto (dirección, teléfono y correo electrónico) figuran en la sección de contacto de este sitio web.</p>
<h2>¿Qué datos recogemos?</h2>
<ul>
  <li>Datos de identificación y contacto: nombres, apellidos, teléfono y correo electrónico.</li>
  <li>Datos relacionados con su salud que usted decida indicarnos, como el motivo de consulta.</li>
  <li>Datos técnicos de navegación de forma agregada, si se utilizan herramientas de analítica web.</li>
</ul>
<h2>¿Para qué usamos sus datos?</h2>
<ul>
  <li>Gestionar su solicitud de cita y comunicarnos con usted para confirmarla o reprogramarla.</li>
  <li>Preparar su atención médica.</li>
  <li>Cumplir las obligaciones legales aplicables a la actividad médica.</li>
</ul>
<p>Sus datos no se usarán con fines publicitarios ni se venderán ni cederán a terceros, salvo obligación legal o cuando sea necesario para su atención (por ejemplo, la clínica o la aseguradora que usted indique).</p>
<h2>Base del tratamiento</h2>
<p>Tratamos sus datos con su consentimiento, que otorga al enviar el formulario de citas aceptando esta política. Los datos de salud se consideran datos sensibles y se tratan con especial reserva y medidas de seguridad.</p>
<h2>¿Cuánto tiempo los conservamos?</h2>
<p>Conservamos sus datos mientras sean necesarios para la finalidad indicada y durante los plazos exigidos por la normativa sanitaria.</p>
<h2>Sus derechos</h2>
<p>Usted puede ejercer en cualquier momento sus derechos de acceso, rectificación, cancelación y oposición (derechos ARCO), así como revocar su consentimiento, escribiendo al correo electrónico indicado en la sección de contacto. Si considera que sus derechos no han sido atendidos, puede acudir a la Autoridad Nacional de Protección de Datos Personales.</p>
<h2>Seguridad</h2>
<p>Aplicamos medidas técnicas y organizativas para proteger sus datos frente a accesos no autorizados, pérdida o alteración.</p>
<h2>Cambios en esta política</h2>
<p>Podemos actualizar esta política. La versión vigente es la publicada en esta página.</p>', true, NULL, 'Cómo tratamos y protegemos los datos personales y de salud de nuestros pacientes, conforme a la Ley N.° 29733.', NULL, true, 'UsrSurgeon', '2026-11-21 20:01:01.45', '::1', false);
INSERT INTO public.paginas_legales (id, clave, ruta, titulo, contenido_html, publicada, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (2, 'terminos', '/terminos', 'Términos de uso del sitio web', '<p>Al usar este sitio web usted acepta los siguientes términos.</p>
<h2>Carácter informativo</h2>
<p>La información publicada sobre enfermedades, procedimientos y cuidados tiene fines exclusivamente informativos y educativos. <strong>No reemplaza una consulta médica</strong>, un diagnóstico ni un tratamiento personalizado.</p>
<h2>Emergencias</h2>
<p>Este sitio web no es un canal de atención de emergencias. Ante un dolor intenso, fiebre alta, sangrado u otro síntoma grave, acuda de inmediato al servicio de emergencia más cercano o llame al SAMU (106).</p>
<h2>Solicitud de citas</h2>
<p>El envío del formulario es una solicitud: la cita queda confirmada solo cuando nos comunicamos con usted. Los precios publicados son referenciales y pueden variar según la evaluación de cada caso.</p>
<h2>Propiedad del contenido</h2>
<p>Los textos, imágenes y logotipos de este sitio pertenecen a su titular. No se permite su reproducción sin autorización.</p>
<h2>Enlaces externos</h2>
<p>Este sitio puede incluir enlaces a otros sitios (por ejemplo, mapas o redes sociales), cuyo contenido no controlamos.</p>
<h2>Cambios</h2>
<p>Podemos modificar estos términos en cualquier momento. La versión vigente es la publicada en esta página.</p>', true, NULL, 'Condiciones de uso de este sitio web: carácter informativo del contenido, citas y responsabilidad.', NULL, true, 'UsrSurgeon', '2026-11-21 20:01:01.457', '::1', false);
INSERT INTO public.paginas_legales (id, clave, ruta, titulo, contenido_html, publicada, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (3, 'consentimiento', '/consentimiento-informado', 'Consentimiento informado', '<p>Antes de cualquier cirugía o procedimiento, usted tiene derecho a recibir información completa y comprensible sobre su situación de salud y a decidir libremente, conforme a la Ley N.° 26842, Ley General de Salud.</p>
<h2>¿Qué información recibirá?</h2>
<ul>
  <li>Su diagnóstico y la finalidad del procedimiento propuesto.</li>
  <li>En qué consiste el procedimiento y el tipo de anestesia.</li>
  <li>Los beneficios esperados, los riesgos frecuentes y los riesgos graves.</li>
  <li>Las alternativas de tratamiento disponibles, incluida la opción de no operarse.</li>
  <li>Los cuidados y la recuperación posteriores.</li>
</ul>
<h2>¿Cómo se otorga?</h2>
<p>El consentimiento informado se firma por escrito en la consulta o en la clínica, después de que el médico le haya explicado el procedimiento y resuelto sus dudas. Ninguna solicitud realizada en este sitio web reemplaza ese documento.</p>
<h2>Sus derechos</h2>
<p>Puede hacer todas las preguntas que necesite, pedir una segunda opinión y revocar su consentimiento antes del procedimiento.</p>', true, NULL, 'Qué es el consentimiento informado y cómo se obtiene antes de cualquier procedimiento quirúrgico.', NULL, true, 'UsrSurgeon', '2026-11-21 20:01:01.46', '::1', false);


-- Data for Name: paginas_seo; Type: TABLE DATA; Schema: public; Owner: -

INSERT INTO public.paginas_seo (id, clave, ruta, nombre_pagina, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (1, 'inicio', '/', 'Portada', 'Cirugía General y Laparoscópica en Lima | Dr. Juan Carlos Marcos', 'Tratamiento quirúrgico avanzado de vesícula, hernias abdominales, apendicitis y patologías digestivas con técnicas de mínima invasión y rápida recuperación.', NULL, true, 'UsrSurgeon', '2026-11-21 18:48:14.377', '::1', false);
INSERT INTO public.paginas_seo (id, clave, ruta, nombre_pagina, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (2, 'procedimientos', '/procedimientos', 'Listado de procedimientos', 'Procedimientos Quirúrgicos en Lima', 'Cirugía de vesícula, hernias, apendicitis, lipomas y consulta de valoración con cirugía laparoscópica de mínima invasión en San Isidro, Lima.', NULL, true, 'UsrSurgeon', '2026-11-21 18:48:14.377', '::1', false);
INSERT INTO public.paginas_seo (id, clave, ruta, nombre_pagina, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (3, 'blog', '/blog', 'Blog médico', 'Blog de Cirugía General y Salud Digestiva', 'Artículos del Dr. Juan Carlos Marcos sobre vesícula, hernias, apendicitis, recuperación postoperatoria y salud digestiva.', NULL, true, 'UsrSurgeon', '2026-11-21 18:48:14.377', '::1', false);
INSERT INTO public.paginas_seo (id, clave, ruta, nombre_pagina, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (4, 'perfil-doctor', '/dr-juan-carlos-marcos', 'Perfil del doctor', 'Dr. Juan Carlos Marcos Enriquez – Cirujano General en Lima', 'Conoce al Dr. Juan Carlos Marcos Enriquez, cirujano general y del aparato digestivo, especialista en cirugía laparoscópica de mínima invasión en San Isidro, Lima.', NULL, true, 'UsrSurgeon', '2026-11-21 18:48:14.377', '::1', false);
INSERT INTO public.paginas_seo (id, clave, ruta, nombre_pagina, meta_titulo, meta_descripcion, imagen_og_url, indexable, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (5, 'contacto', '/contacto', 'Contacto', 'Contacto y Ubicación del Consultorio en San Isidro', 'Consultorio del Dr. Juan Carlos Marcos Enriquez en San Isidro, Lima. Teléfonos, horarios de atención, cómo llegar y reserva de citas.', NULL, true, 'admin', '2026-11-21 19:11:09.693', '0.0.0.1', false);


-- Data for Name: redirecciones; Type: TABLE DATA; Schema: public; Owner: -



-- Data for Name: roles; Type: TABLE DATA; Schema: public; Owner: -

INSERT INTO public.roles (id, nombre, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (1, 'Administrador', 'UsrSurgeon', '2026-11-21 18:48:14.12', '::1', false);
INSERT INTO public.roles (id, nombre, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (2, 'Cirujano', 'UsrSurgeon', '2026-11-21 18:48:14.12', '::1', false);
INSERT INTO public.roles (id, nombre, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (3, 'Secretaria', 'UsrSurgeon', '2026-11-21 18:48:14.12', '::1', false);
INSERT INTO public.roles (id, nombre, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (4, 'Paciente', 'UsrSurgeon', '2026-11-21 18:48:14.12', '::1', false);


-- Data for Name: servicios_contenido; Type: TABLE DATA; Schema: public; Owner: -

INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (21, 2, 'Introduccion', NULL, 'Una hernia es la salida de grasa o intestino a través de un punto débil de la pared abdominal. Se nota como un bulto en la ingle, el ombligo o una cicatriz previa, que suele aumentar al toser, cargar peso o estar de pie.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (22, 2, 'Introduccion', NULL, 'Las hernias no se curan solas ni con fajas: con el tiempo tienden a crecer. La reparación quirúrgica con malla refuerza la pared abdominal y reduce de forma importante el riesgo de que la hernia vuelva a aparecer.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (23, 2, 'Indicacion', NULL, 'Bulto en la ingle, el ombligo o sobre una cicatriz que aparece al hacer esfuerzo y desaparece al acostarse.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (24, 2, 'Indicacion', NULL, 'Dolor, ardor o sensación de peso en la zona, sobre todo al final del día.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (25, 2, 'Indicacion', NULL, 'Molestia al toser, levantar objetos o hacer ejercicio.', 3, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (26, 2, 'Indicacion', NULL, 'Urgencia: bulto duro, doloroso que no se reduce, con náuseas o vómitos (posible hernia atascada).', 4, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (27, 2, 'Paso', 'Evaluación', 'Examen físico y, si es necesario, ecografía para definir el tipo y tamaño de la hernia.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (28, 2, 'Paso', 'Elección de técnica', 'Según el caso, reparación laparoscópica (pequeñas incisiones) o abierta, ambas con malla.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (29, 2, 'Paso', 'Cirugía', 'Se devuelve el contenido herniado a su lugar y se coloca una malla que refuerza la pared. Dura alrededor de 1 hora.', 3, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (30, 2, 'Paso', 'Alta y controles', 'En la mayoría de casos el alta es el mismo día o al día siguiente, con controles programados.', 4, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (31, 2, 'Recuperacion', NULL, 'Caminar desde el mismo día.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (32, 2, 'Recuperacion', NULL, 'Actividades ligeras y trabajo de oficina en 7 a 10 días.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (33, 2, 'Recuperacion', NULL, 'Evitar cargar peso y esfuerzos abdominales durante 4 a 6 semanas.', 3, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (34, 2, 'Recuperacion', NULL, 'Dolor controlable con analgésicos comunes.', 4, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (35, 2, 'Pregunta', '¿Una hernia puede desaparecer sola?', 'No. Las fajas pueden aliviar molestias, pero no cierran el defecto de la pared abdominal; la única solución definitiva es la cirugía.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (36, 2, 'Pregunta', '¿La malla es segura?', 'Sí. Las mallas modernas son biocompatibles y son el estándar internacional porque disminuyen notablemente la recurrencia de la hernia.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (37, 2, 'Pregunta', '¿Cuándo es una urgencia?', 'Si el bulto se pone duro, muy doloroso, cambia de color o no regresa al acostarse, especialmente con vómitos. En ese caso hay que acudir de inmediato.', 3, 'UsrSurgeon', '2026-11-21 18:48:14.407', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (38, 3, 'Introduccion', NULL, 'La apendicitis es la inflamación del apéndice y es una de las urgencias quirúrgicas más frecuentes. Su tratamiento es la apendicectomía: la extirpación del apéndice.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (39, 3, 'Introduccion', NULL, 'Operar a tiempo es clave. Si la apendicitis avanza, el apéndice puede perforarse y causar una infección en el abdomen (peritonitis). Por vía laparoscópica, la cirugía se realiza con pequeñas incisiones y una recuperación más rápida.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (40, 3, 'Indicacion', NULL, 'Dolor que empieza alrededor del ombligo y se desplaza a la parte baja derecha del abdomen.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (41, 3, 'Indicacion', NULL, 'Dolor que empeora al caminar, toser o saltar.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (42, 3, 'Indicacion', NULL, 'Pérdida de apetito, náuseas o vómitos.', 3, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (43, 3, 'Indicacion', NULL, 'Fiebre leve que puede aumentar con las horas.', 4, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (44, 3, 'Indicacion', NULL, 'Ante estos síntomas, no tomar analgésicos ni laxantes y acudir a evaluación médica.', 5, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (45, 3, 'Paso', 'Diagnóstico rápido', 'Examen físico, análisis de sangre y ecografía o tomografía para confirmar la apendicitis.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (46, 3, 'Paso', 'Cirugía laparoscópica', 'Bajo anestesia general se retira el apéndice por pequeñas incisiones. Dura alrededor de 45 minutos.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (47, 3, 'Paso', 'Hospitalización corta', 'En apendicitis no complicada, el alta suele ser en 24 a 48 horas.', 3, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (48, 3, 'Paso', 'Seguimiento', 'Control de heridas y retorno progresivo a la actividad.', 4, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (49, 3, 'Recuperacion', NULL, 'Caminar desde las primeras horas tras la cirugía.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (50, 3, 'Recuperacion', NULL, 'Dieta progresiva iniciando el mismo día o al siguiente.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (51, 3, 'Recuperacion', NULL, 'Retorno a estudios o trabajo de oficina en 1 a 2 semanas.', 3, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (52, 3, 'Recuperacion', NULL, 'Evitar esfuerzos físicos intensos durante 3 a 4 semanas.', 4, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (53, 3, 'Pregunta', '¿La apendicitis siempre requiere cirugía?', 'En la gran mayoría de casos sí: la apendicectomía es el tratamiento estándar y el más seguro para evitar complicaciones.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (54, 3, 'Pregunta', '¿Cuánto tiempo puedo esperar con dolor?', 'No se debe esperar. Si sospechas apendicitis, busca atención médica en las primeras horas: el riesgo de perforación aumenta con el tiempo.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (55, 3, 'Pregunta', '¿Se puede vivir sin apéndice?', 'Sí, sin ninguna consecuencia para la salud ni la digestión.', 3, 'UsrSurgeon', '2026-11-21 18:48:14.413', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (56, 4, 'Introduccion', NULL, 'La consulta de valoración es el primer paso antes de cualquier cirugía. En ella se revisan tus síntomas, antecedentes y exámenes para llegar a un diagnóstico preciso y decidir si realmente necesitas operarte.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (57, 4, 'Introduccion', NULL, 'También es el espacio ideal para pedir una segunda opinión: se explican con claridad las alternativas, los riesgos y los beneficios de cada opción, sin recomendar cirugías innecesarias.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (58, 4, 'Indicacion', NULL, 'Dolor abdominal recurrente o molestias digestivas sin diagnóstico claro.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (59, 4, 'Indicacion', NULL, 'Te detectaron cálculos en la vesícula, una hernia o un pólipo en una ecografía.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (60, 4, 'Indicacion', NULL, 'Te indicaron una cirugía y quieres una segunda opinión.', 3, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (61, 4, 'Indicacion', NULL, 'Tienes un bulto en la piel (lipoma, quiste) que crece o molesta.', 4, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (62, 4, 'Indicacion', NULL, 'Necesitas evaluación preoperatoria o seguimiento después de una cirugía.', 5, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (63, 4, 'Paso', 'Historia clínica', 'Conversamos sobre tus síntomas, antecedentes y medicamentos.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (64, 4, 'Paso', 'Examen físico', 'Evaluación dirigida según el motivo de consulta.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (65, 4, 'Paso', 'Revisión de exámenes', 'Se revisan ecografías, tomografías y análisis que traigas; si hace falta, se solicitan nuevos.', 3, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (66, 4, 'Paso', 'Plan de tratamiento', 'Recibes un diagnóstico y un plan claro: tratamiento médico, observación o cirugía programada.', 4, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (67, 4, 'Recuperacion', NULL, 'Trae tus exámenes previos (ecografías, tomografías, análisis) impresos o en digital.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (68, 4, 'Recuperacion', NULL, 'Lleva la lista de medicamentos que tomas habitualmente.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (69, 4, 'Recuperacion', NULL, 'Anota tus dudas para resolverlas todas en la consulta.', 3, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (70, 4, 'Pregunta', '¿Necesito traer exámenes?', 'No es obligatorio, pero si los tienes ayudan a agilizar el diagnóstico. Si faltan, se solicitarán en la consulta.', 1, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (71, 4, 'Pregunta', '¿Atienden con seguro?', 'Trabajamos con seguros aliados. Consulta tu cobertura al agendar tu cita.', 2, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (72, 4, 'Pregunta', '¿Puedo pedir una segunda opinión?', 'Sí. Es una de las razones más frecuentes de consulta y es tu derecho como paciente antes de una cirugía.', 3, 'UsrSurgeon', '2026-11-21 18:48:14.423', '::1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (90, 1, 'Introduccion', NULL, 'La colecistectomía laparoscópica es la cirugía para extirpar la vesícula biliar cuando presenta cálculos (piedras), inflamación o pólipos. Es el tratamiento definitivo de la colelitiasis: los cálculos no desaparecen con dietas ni medicamentos.', 1, 'admin', '2026-11-21 19:00:31.207', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (91, 1, 'Introduccion', NULL, 'Se realiza a través de pequeñas incisiones de 5 a 10 mm con una cámara de alta definición, lo que permite menos dolor, una estancia hospitalaria corta y un retorno rápido a las actividades diarias. El cuerpo funciona con normalidad sin vesícula: el hígado sigue produciendo la bilis necesaria para la digestión.', 2, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (92, 1, 'Indicacion', NULL, 'Dolor en la parte superior derecha del abdomen después de comer, sobre todo comidas grasas (cólico biliar).', 1, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (93, 1, 'Indicacion', NULL, 'Náuseas, vómitos, hinchazón o pesadez frecuentes tras las comidas.', 2, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (94, 1, 'Indicacion', NULL, 'Cálculos o barro biliar confirmados por ecografía, aunque den pocas molestias.', 3, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (95, 1, 'Indicacion', NULL, 'Episodios previos de colecistitis (vesícula inflamada) o pancreatitis biliar.', 4, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (96, 1, 'Indicacion', NULL, 'Pólipos vesiculares de tamaño significativo.', 5, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (97, 1, 'Indicacion', NULL, 'Coloración amarilla de piel u ojos (ictericia): requiere evaluación urgente.', 6, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (98, 1, 'Paso', 'Evaluación y exámenes', 'Consulta, ecografía abdominal y análisis de sangre preoperatorios. Se revisa el riesgo quirúrgico de forma individual.', 1, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (99, 1, 'Paso', 'Cirugía laparoscópica', 'Bajo anestesia general, mediante 3 o 4 pequeñas incisiones, se retira la vesícula con sus cálculos. Dura alrededor de 1 hora.', 2, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (100, 1, 'Paso', 'Alta temprana', 'La mayoría de pacientes camina el mismo día y se va a casa en 24 horas, con indicaciones de dieta y analgesia.', 3, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (101, 1, 'Paso', 'Control postoperatorio', 'Controles para revisar las heridas y el resultado de patología de la vesícula.', 4, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (102, 1, 'Recuperacion', NULL, 'Caminar desde el mismo día de la cirugía.', 1, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (103, 1, 'Recuperacion', NULL, 'Dieta baja en grasas las primeras semanas, con retorno progresivo a la alimentación normal.', 2, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (104, 1, 'Recuperacion', NULL, 'Trabajo de oficina en aproximadamente 7 días.', 3, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (105, 1, 'Recuperacion', NULL, 'Evitar cargar peso o hacer ejercicio intenso durante 3 a 4 semanas.', 4, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (106, 1, 'Pregunta', '¿Se puede vivir normalmente sin vesícula?', 'Sí. La vesícula solo almacena la bilis; tras la cirugía el hígado la sigue produciendo y la vierte directamente al intestino. La gran mayoría de personas no nota diferencias a largo plazo.', 1, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (107, 1, 'Pregunta', '¿Los cálculos se pueden eliminar con medicamentos o dietas?', 'No de forma confiable. Las dietas pueden reducir los síntomas, pero los cálculos permanecen y pueden causar complicaciones como colecistitis o pancreatitis. La cirugía es el tratamiento definitivo.', 2, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (108, 1, 'Pregunta', '¿Cuánto dura la operación de vesícula?', 'Habitualmente alrededor de una hora, y el alta suele darse en 24 horas.', 3, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (109, 1, 'Pregunta', '¿Qué pasa si no me opero?', 'Los síntomas tienden a repetirse y aumentan el riesgo de complicaciones que pueden requerir una cirugía de urgencia, más compleja que una programada.', 4, 'admin', '2026-11-21 19:00:31.217', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (127, 5, 'Introduccion', NULL, 'Los lipomas (bultos de grasa bajo la piel), los quistes sebáceos y algunos lunares pueden retirarse con una cirugía menor, rápida y ambulatoria, usando anestesia local.', 1, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (128, 5, 'Introduccion', NULL, 'Se recomienda extirparlos cuando crecen, duelen, se inflaman, molestan con la ropa o por estética. La lesión se envía a análisis (patología) para confirmar que es benigna.', 2, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (129, 5, 'Indicacion', NULL, 'Bulto bajo la piel que ha crecido con el tiempo.', 1, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (130, 5, 'Indicacion', NULL, 'Quiste que se inflama, duele o drena material.', 2, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (131, 5, 'Indicacion', NULL, 'Lunar que cambia de forma, color o tamaño, o que sangra.', 3, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (132, 5, 'Indicacion', NULL, 'Lesiones que molestan con la ropa o por motivos estéticos.', 4, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (133, 5, 'Paso', 'Evaluación', 'Se examina la lesión y, si hace falta, se pide una ecografía de partes blandas.', 1, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (134, 5, 'Paso', 'Anestesia local', 'Se adormece solo la zona; el paciente está despierto y sin dolor.', 2, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (135, 5, 'Paso', 'Extirpación', 'Se retira la lesión completa y se cierra con puntos finos cuidando la cicatriz. Dura unos 30 minutos.', 3, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (136, 5, 'Paso', 'Patología y retiro de puntos', 'La muestra se analiza y los puntos se retiran en 7 a 10 días.', 4, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (137, 5, 'Recuperacion', NULL, 'Regreso a casa inmediatamente después del procedimiento.', 1, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (138, 5, 'Recuperacion', NULL, 'Actividades normales al día siguiente en la mayoría de casos.', 2, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (139, 5, 'Recuperacion', NULL, 'Curaciones sencillas en casa según indicaciones.', 3, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (140, 5, 'Recuperacion', NULL, 'Proteger la cicatriz del sol durante algunos meses.', 4, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (141, 5, 'Pregunta', '¿Duele el procedimiento?', 'No. Con anestesia local solo se siente el pinchazo inicial; durante la cirugía no hay dolor.', 1, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (142, 5, 'Pregunta', '¿Queda cicatriz?', 'Queda una cicatriz fina, que se procura orientar y cerrar con técnica estética para que sea lo menos visible posible.', 2, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);
INSERT INTO public.servicios_contenido (id, id_servicio_medico, tipo, titulo, texto, orden, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (143, 5, 'Pregunta', '¿Puede volver a salir?', 'Si la lesión se retira completa, la recurrencia es poco frecuente.', 3, 'admin', '2026-11-21 19:11:09.897', '0.0.0.1', false);


-- Data for Name: testimonios; Type: TABLE DATA; Schema: public; Owner: -

INSERT INTO public.testimonios (id, nombre_paciente, procedimiento, id_servicio_medico, calificacion_estrellas, comentario, fecha_testimonio, aprobado_para_web, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (1, 'Carlos Alberto Ramos', 'Cirugía de Vesícula Laparoscópica', 1, 5, 'Excelente atención del doctor. Me operé el martes y para el jueves ya estaba caminando sin dolor en casa. Muy agradecido por su profesionalismo y amabilidad.', '2026-11-21 00:00:00', true, 'UsrSurgeon', '2026-11-21 18:48:14.143', '::1', false);
INSERT INTO public.testimonios (id, nombre_paciente, procedimiento, id_servicio_medico, calificacion_estrellas, comentario, fecha_testimonio, aprobado_para_web, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (2, 'María Fernanda López', 'Reparación de Hernia Umbilical', 2, 5, 'Me explicó todo el procedimiento con mucha paciencia en la consulta de valoración. La cirugía fue un éxito y la cicatriz apenas se nota. 100% recomendado.', '2026-11-21 00:00:00', true, 'UsrSurgeon', '2026-11-21 18:48:14.143', '::1', false);
INSERT INTO public.testimonios (id, nombre_paciente, procedimiento, id_servicio_medico, calificacion_estrellas, comentario, fecha_testimonio, aprobado_para_web, usuario_modificacion, fecha_modificacion, ip_modificacion, eliminado) VALUES (3, 'Roberto Gómez', 'Consulta de Valoración Quirúrgica', 4, 5, 'Muy puntual y certero en su diagnóstico. Evitó una cirugía innecesaria con el tratamiento médico adecuado.', '2026-11-21 00:00:00', true, 'UsrSurgeon', '2026-11-21 18:48:14.143', '::1', false);


-- Name: articulos_blog_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.articulos_blog_id_seq', 3, true);


-- Name: categorias_blog_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.categorias_blog_id_seq', 7, true);


-- Name: configuracion_sitio_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.configuracion_sitio_id_seq', 1, true);


-- Name: especialidades_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.especialidades_id_seq', 4, true);


-- Name: estados_cita_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.estados_cita_id_seq', 4, true);


-- Name: horarios_atencion_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.horarios_atencion_id_seq', 22, true);


-- Name: paginas_legales_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.paginas_legales_id_seq', 3, true);


-- Name: paginas_seo_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.paginas_seo_id_seq', 5, true);


-- Name: redirecciones_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.redirecciones_id_seq', 1, false);


-- Name: roles_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.roles_id_seq', 4, true);


-- Name: servicios_contenido_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.servicios_contenido_id_seq', 143, true);


-- Name: servicios_medicos_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.servicios_medicos_id_seq', 5, true);


-- Name: testimonios_id_seq; Type: SEQUENCE SET; Schema: public; Owner: -

SELECT pg_catalog.setval('public.testimonios_id_seq', 3, true);



