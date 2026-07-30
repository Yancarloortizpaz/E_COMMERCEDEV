USE [DB_EcommerceAgent];
GO


-- 1. REGLAS DEL SISTEMA EXPERTO (Identificadores del 1 al 6 estables)

INSERT INTO ReglasChatbot (NombreRegla, AccionDinamica, AccionPython, Activo)
VALUES 
('Saludo Inicial', 1, 'cargar_saludos_db', 1),
('Buscar Producto', 1, 'buscar_producto_en_db', 1),
('Metodos de Pago', 0, NULL, 1),
('Despedida', 0, NULL, 1),
('Sin Resultados', 0, NULL, 1),
('No Reconocido', 0, NULL, 1);


-- 2. PALABRAS CLAVE (TRIGGERS ADMINISTRATIVOS DE CONTROL EXCLUSIVO)

INSERT INTO PalabrasClaveRegla (ReglaID, PalabraClave, Activo)
VALUES 
-- Triggers para Saludo (ReglaID = 1)
(1, 'hola', 1), (1, 'buenos dias', 1), (1, 'buenas tardes', 1), (1, 'buenas noches', 1), (1, 'que tal', 1),

-- Triggers para Métodos de Pago (ReglaID = 3)
(3, 'pagar', 1), (3, 'pago', 1), (3, 'tarjeta', 1), (3, 'efectivo', 1), (3, 'transferencia', 1), (3, 'puedo pagar', 1),

-- Triggers para Despedida (ReglaID = 4)
(4, 'adios', 1), (4, 'gracias', 1), (4, 'chao', 1), (4, 'hasta luego', 1);

-- Nota: La Regla 2 (Buscar Producto) ya no tiene triggers fijos aquí. 
-- Será evaluada dinámicamente mediante el descarte de las anteriores.


-- 3. PLANTILLAS DE RESPUESTA DE VARIACIÓN ALEATORIA

-- Saludo Inicial (ReglaID = 1)
INSERT INTO PlantillasRespuesta (ReglaID, TextoRespuesta, Activo)
VALUES 
(1, '¡Hola! Bienvenido a nuestra tienda virtual. ¿Qué producto deseas buscar hoy?', 1),
(1, '¡Qué gusto saludarte! ¿Buscas algún artículo o marca en nuestro catálogo en este momento?', 1);

-- Buscar Producto con marcador [@TABLA] (ReglaID = 2)
INSERT INTO PlantillasRespuesta (ReglaID, TextoRespuesta, Activo)
VALUES 
(2, '¡Perfecto! He consultado nuestro inventario en tiempo real y encontré estas opciones para ti: \n [@TABLA]', 1),
(2, 'Claro que sí, aquí tienes los detalles de los productos disponibles que coinciden con tu búsqueda: \n [@TABLA]', 1);

-- Métodos de Pago (ReglaID = 3)
INSERT INTO PlantillasRespuesta (ReglaID, TextoRespuesta, Activo)
VALUES 
(3, 'Contamos con múltiples opciones de pago: Tarjetas de crédito/débito, transferencias y pagos seguros integrados.', 1);

-- Despedida (ReglaID = 4)
INSERT INTO PlantillasRespuesta (ReglaID, TextoRespuesta, Activo)
VALUES 
(4, '¡Fue un placer ayudarte! Si deseas buscar otro producto, solo escribe su nombre aquí.', 1),
(4, 'Gracias por escribirnos. ¡Vuelve pronto!', 1);

-- Sin Resultados (ReglaID = 5)
INSERT INTO PlantillasRespuesta (ReglaID, TextoRespuesta, Activo)
VALUES 
(5, 'Disculpa, busqué en nuestro catálogo pero actualmente no contamos con existencias de ese producto.', 1),
(5, 'Por el momento no tenemos disponible ese artículo en nuestro inventario. ¿Te gustaría buscar otra marca o producto?', 1);

-- No Reconocido / Fallback estricto (ReglaID = 6)
INSERT INTO PlantillasRespuesta (ReglaID, TextoRespuesta, Activo)
VALUES 
(6, 'Lo siento, no logré entender tu consulta. Recuerda que puedo ayudarte a buscar cualquier producto de nuestro catálogo o indicarte los métodos de pago.', 1),
(6, 'Vaya, no reconozco esa instrucción. ¿Podrías intentar escribiendo el nombre de un producto o marca (ej. Dell, Nike, Zapatillas)?', 1);
GO


-------------------- nuevas reglas 



INSERT INTO ReglasChatbot (NombreRegla, AccionDinamica, AccionPython, Activo)
VALUES 
('Agregar Carrito', 1, 'agregar_carrito', 1),
('Consultar Carrito', 1, 'consultar_carrito', 1),
('Eliminar Producto Carrito', 1, 'eliminar_carrito', 1),
('Procesar Pago', 1, 'procesar_pago', 1),
('Consultar Orden', 1, 'consultar_orden', 1);

-- Palabras Clave Triggers (Asociadas a ReglaID 7-11)
INSERT INTO PalabrasClaveRegla (ReglaID, PalabraClave, Activo)
VALUES 
-- Regla 7 (Agregar Carrito)
(8, 'agregar al carrito', 1), (7, 'anadir al carrito', 1), (7, 'añadir al carrito', 1), (7, 'agregar carrito', 1),
-- Regla 8 (Consultar Carrito)
(9, 'ver carrito', 1), (8, 'mi carrito', 1), (8, 'consultar carrito', 1), (8, 'carrito', 1),
-- Regla 9 (Eliminar Producto)
(10, 'eliminar del carrito', 1), (9, 'quitar del carrito', 1), (9, 'borrar del carrito', 1),
-- Regla 10 (Procesar Pago)
(11, 'procesar pago', 1), (10, 'finalizar compra', 1), (10, 'pagar carrito', 1), (10, 'pagar', 1),
-- Regla 11 (Consultar Orden)
(12, 'estado de mi orden', 1), (11, 'mi orden', 1), (11, 'ver pedido', 1), (11, 'rastrear orden', 1);


