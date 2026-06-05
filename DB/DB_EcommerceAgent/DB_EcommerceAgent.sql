CREATE DATABASE [DB_EcommerceAgent]
GO

USE [DB_EcommerceAgent]
GO

-- 1. CREACIÓN DE LA TABLA PRINCIPAL: REGLAS
-- Aquí guardamos QUÉ debe hacer el bot cuando se activa una regla.
CREATE TABLE ReglasChatbot (
    ReglaID INT PRIMARY KEY IDENTITY(1,1),
    NombreRegla VARCHAR(100) NOT NULL,
    TipoRespuesta VARCHAR(50) NOT NULL,     -- 'TEXTO_ESTATICO' o 'ACCION_DINAMICA'
    AccionPython VARCHAR(100) NULL,         -- Nombre de la función en Python (si es dinámica)
    Activo BIT                    -- Para activar/desactivar reglas fácilmente
);

-- 2. CREACIÓN DE LA TABLA SECUNDARIA: PALABRAS CLAVE (TRIGGERS)
-- Aquí guardamos las palabras que el usuario podría escribir para activar la regla.
CREATE TABLE PalabrasClaveRegla (
    PalabraClaveID INT PRIMARY KEY IDENTITY(1,1),
    ReglaID INT NOT NULL REFERENCES ReglasChatbot(ReglaID),
    PalabraClave VARCHAR(100) NOT NULL,
	Activo BIT
);

-- =====================================================================
-- 1. TABLA: VARIACIONES DE RESPUESTAS (PLANTILLAS DE SALIDA)
-- Modificamos el enfoque anterior para que una regla tenga MUCHAS opciones de respuesta.
-- =====================================================================

-- Primero, una buena práctica: si ya creaste la tabla 'ReglasChatbot' con la columna 'RespuestaTexto',
-- la eliminamos de ahí porque ahora vivirá de forma más organizada en esta nueva tabla.
CREATE TABLE PlantillasRespuesta (
    PlantillaID INT IDENTITY(1,1) PRIMARY KEY,
    ReglaID INT NOT NULL REFERENCES ReglasChatbot(ReglaID),
    TextoRespuesta NVARCHAR(MAX) NOT NULL, -- La frase exacta que dirá el bot
    Activo BIT
);


-- =====================================================================
-- 2. TABLA: HISTORIAL DE CONVERSACIONES (LOGS DE ENTRADA Y SALIDA)
-- Aquí se almacena la interacción real del e-commerce. Todo lo que entra y sale.
-- =====================================================================
CREATE TABLE HistorialMensajes (
    MensajeID BIGINT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID VARCHAR(100) NOT NULL,          -- El ID del cliente en tu app móvil
    Remitente VARCHAR(10) NOT NULL,           -- 'USUARIO' o 'SISTEMA'
    Texto NVARCHAR(MAX) NOT NULL,             -- El mensaje de texto enviado o recibido
    FechaHora DATETIME NOT NULL,     -- Momento exacto de la interacción
    ReglaActivadaID INT REFERENCES ReglasChatbot(ReglaID),               -- Qué regla del sistema experto respondió (si fue el SISTEMA)       
	Activo BIT
);


-- =====================================================================
-- 3. DATOS DE PRUEBA PARA LAS NUEVAS TABLAS
-- =====================================================================

-- 3. INSERCIÓN DE DATOS DE PRUEBA (Caso E-commerce)
-- Insertamos primero las Reglas del Sistema Experto
INSERT INTO ReglasChatbot (NombreRegla, TipoRespuesta, AccionPython, Activo)
VALUES 
('Saludo Inicial', 'ACCION_DINAMICA',  'buscar_saludos_en_db', 1),
('Buscar Producto', 'ACCION_DINAMICA', 'buscar_producto_en_db',1),
('Soporte Humano', 'TEXTO_ESTATICO', NULL,1);

-- Insertamos las Palabras Clave asociadas a cada Regla (usando los IDs generados)
-- Suponiendo que 'Saludo Inicial' es ID 1, 'Buscar Producto' es ID 2, y 'Soporte Humano' es ID 3
INSERT INTO PalabrasClaveRegla (ReglaID, PalabraClave, Activo)
VALUES 
(1, 'hola',1),
(1, 'buenos dias',1),
(1, 'buenas tardes',1),
(2, 'tienen',1),
(2, 'buscar',1),
(2, 'precio',1),
(2, 'stock',1),
(3, 'asesor',1),
(3, 'humano',1),
(3, 'ayuda',1);

-- Agregamos múltiples opciones de respuesta para el 'Saludo Inicial' (Asumiendo que es el ReglaID = 1)
INSERT INTO PlantillasRespuesta (ReglaID, TextoRespuesta, Activo)
VALUES 
(1, '¡Hola! Bienvenido a nuestra tienda. ¿En qué te puedo colaborar hoy?', 1),
(1, '¡Qué gusto tenerte de vuelta! ¿Buscas algún producto en nuestro catálogo?',1),
(1, 'Hola, soy tu asistente de compras virtuales. ¿Deseas buscar un artículo o ver el estado de un pedido?',1);

-- Simulación de un log en el Historial de cómo se guardaría una conversación real:
INSERT INTO HistorialMensajes (UsuarioID, Remitente, Texto, ReglaActivadaID, FechaHora, Activo)
VALUES 
('usr_mobile_8842', 'USUARIO', 'Hola, buenos días', NULL,GETDATE(),1),
('usr_mobile_8842', 'SISTEMA', '¡Qué gusto tenerte de vuelta! ¿Buscas algún producto en nuestro catálogo?', 1,GETDATE(), 1);

SELECT *
FROM ReglasChatbot

SELECT *
FROM PalabrasClaveRegla

SELECT *
FROM PlantillasRespuesta

SELECT *
FROM HistorialMensajes