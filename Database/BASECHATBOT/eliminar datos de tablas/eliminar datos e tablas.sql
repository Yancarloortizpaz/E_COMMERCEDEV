USE [DB_EcommerceAgent];
GO

-- 1. ELIMINAMOS LOGS E HISTORIALES DE CHATS PRIMERO (Por dependencias)
TRUNCATE TABLE HistorialMensajes;

-- Para HistorialConversaciones usamos DELETE porque podría estar referenciada por FK activas, 
-- pero reiniciamos el contador manualmente a 0 para que el próximo sea 1.
DELETE FROM HistorialConversaciones;
DBCC CHECKIDENT ('HistorialConversaciones', RESEED, 0);

-- 2. ELIMINAMOS REGLAS, PALABRAS CLAVE Y PLANTILLAS
TRUNCATE TABLE PalabrasClaveRegla;
TRUNCATE TABLE PlantillasRespuesta;

-- ReglasChatbot está referenciada por FKs, así que la limpiamos con DELETE y reiniciamos su ID
DELETE FROM ReglasChatbot;
DBCC CHECKIDENT ('ReglasChatbot', RESEED, 0);
GO