USE [DB_EcommerceAgent]
GO

CREATE OR ALTER PROCEDURE dbo.SP_ProcesarMensajeChatbot
    @w_ConversacionID BIGINT,
    @w_TextoUsuario VARCHAR(1000),
    @o_TextoRespuesta NVARCHAR(MAX) OUTPUT,
    @o_ReglaActivadaID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. REGISTRAR EL MENSAJE ENTRANTE DEL USUARIO
    IF NOT EXISTS (SELECT 1 FROM Conversaciones WHERE ConversacionID = CAST(@w_ConversacionID AS VARCHAR(50)))
    BEGIN
        INSERT INTO Conversaciones (ConversacionID, UsuarioID, FechaInicio, Activo)
        VALUES (CAST(@w_ConversacionID AS VARCHAR(50)), 'UsuarioAnonimo', GETDATE(), 1);
    END

    INSERT INTO Mensajes (ConversacionID, Rol, ChatBot, Contenido, FechaHora, ReglaActivadaID)
    VALUES (CAST(@w_ConversacionID AS VARCHAR(50)), 'user', 0, @w_TextoUsuario, GETDATE(), NULL);

    DECLARE @v_ReglaID INT = NULL;
    DECLARE @v_PlantillaTexto NVARCHAR(MAX) = '';
    DECLARE @v_FiltroTexto VARCHAR(100) = NULL;
    DECLARE @v_EsBusquedaPorDescarte BIT = 0;

    -- 2. EVALUAR SI EL TEXTO CONTIENE UN TRIGGER DE CONTROL (Saludos, Pagos, Despedidas)
    SELECT TOP 1 
        @v_ReglaID = ReglaID
    FROM PalabrasClaveRegla
    WHERE Activo = 1
      AND CHARINDEX(PalabraClave, LOWER(@w_TextoUsuario)) > 0;

    -- 3. INVERSIÓN DE LA REGLA: Si no es un control fijo, asumimos por defecto la intención Buscar Producto (Regla 2)
    IF @v_ReglaID IS NULL
    BEGIN
        SET @v_ReglaID = 2; 
        SET @v_EsBusquedaPorDescarte = 1;
    END

    SET @v_FiltroTexto = TRIM(@w_TextoUsuario);
    SET @o_ReglaActivadaID = @v_ReglaID;

    -- 4. FLUJO DE CONTROL DINÁMICO: REGLA 2 (BUSCAR PRODUCTO)
    IF @v_ReglaID = 2
    BEGIN
        -- Sincronizamos la tabla agregando la columna 22 al final para evitar el Msg 213
        DECLARE @CoincidenceScore TABLE (
            ProductID INT, ProductName VARCHAR(50), ProductVariableID INT, ProductVariableName VARCHAR(50),
            ProductVariablePrice DECIMAL(18,2), CurrencyID INT, CurrencyISO CHAR(5), CategoryID INT,
            CategoryName VARCHAR(50), SubcategoryID INT, SubcategoryName VARCHAR(50), SegmentID INT,
            SegmentName VARCHAR(50), MarkID INT, MarkName VARCHAR(50), ProviderID INT,
            ProviderName VARCHAR(50), StockID INT, StockAvilable INT, StockFactoryDate DATE, StockExpirationDate DATE,
            CoincidenceScore INT -- <-- ¡Esta columna faltaba en tu declaración!
        );

        -- Invocamos al buscador inteligente por token/scoring
        INSERT INTO @CoincidenceScore
        EXEC dbo.SP_ListarGeneralProducts_Filtro @01_FilterText = @v_FiltroTexto;

        -- Validamos si se consiguieron artículos aptos y en stock
        IF EXISTS (SELECT 1 FROM @CoincidenceScore WHERE StockAvilable > 0)
        BEGIN
            SELECT TOP 1 @v_PlantillaTexto = TextoRespuesta FROM PlantillasRespuesta WHERE ReglaID = 2 AND Activo = 1 ORDER BY NEWID();

            DECLARE @v_ListaFormateada NVARCHAR(MAX) = '';
            SELECT @v_ListaFormateada = @v_ListaFormateada + 
                CHAR(13) + CHAR(10) + '- ' + ProductName + ' (' + ProductVariableName + ') | Precio: ' + 
                CurrencyISO + ' ' + CAST(ProductVariablePrice AS VARCHAR) + ' | Stock: ' + CAST(StockAvilable AS VARCHAR)
            FROM @CoincidenceScore
            WHERE StockAvilable > 0;

            SET @o_TextoRespuesta = REPLACE(@v_PlantillaTexto, '[@TABLA]', @v_ListaFormateada);
        END
        ELSE
        BEGIN
            -- Tratamiento de Fallos en las búsquedas
            IF @v_EsBusquedaPorDescarte = 1 AND LEN(@v_FiltroTexto) < 4
            BEGIN
                SELECT TOP 1 @v_PlantillaTexto = TextoRespuesta FROM PlantillasRespuesta WHERE ReglaID = 6 AND Activo = 1 ORDER BY NEWID();
                SET @o_TextoRespuesta = @v_PlantillaTexto;
                SET @o_ReglaActivadaID = 6;
            END
            ELSE
            BEGIN
                SELECT TOP 1 @v_PlantillaTexto = TextoRespuesta FROM PlantillasRespuesta WHERE ReglaID = 5 AND Activo = 1 ORDER BY NEWID();
                SET @o_TextoRespuesta = @v_PlantillaTexto;
                SET @o_ReglaActivadaID = 5;
            END
        END
    END
    ELSE
    BEGIN
        -- 5. FLUJO DE CONTROL ESTÁTICO (Saludos, Métodos de pago, Despedidas)
        SELECT TOP 1 @v_PlantillaTexto = TextoRespuesta FROM PlantillasRespuesta WHERE ReglaID = @v_ReglaID AND Activo = 1 ORDER BY NEWID();
        SET @o_TextoRespuesta = @v_PlantillaTexto;
    END

    -- 6. REGISTRAR LA RESPUESTA FINAL EMITIDA POR EL BOT
    INSERT INTO Mensajes (ConversacionID, Rol, ChatBot, Contenido, FechaHora, ReglaActivadaID)
    VALUES (CAST(@w_ConversacionID AS VARCHAR(50)), 'assistant', 1, LEFT(@o_TextoRespuesta, 1000), GETDATE(), @o_ReglaActivadaID);

END
GO



-- PRUEBA CASO 1: El usuario saluda con una palabra clave ("hola")

DECLARE @RespuestaChatbot NVARCHAR(MAX);
DECLARE @ReglaID INT;

EXEC dbo.SP_ProcesarMensajeChatbot 
    @w_ConversacionID = 1, 
    @w_TextoUsuario = 'Hola buenas tardes', 
    @o_TextoRespuesta = @RespuestaChatbot OUTPUT,
    @o_ReglaActivadaID = @ReglaID OUTPUT;

SELECT @RespuestaChatbot AS [Bot_Dice], @ReglaID AS [Regla_Disparada];
GO



-- PRUEBA CASO 2: El usuario pide una marca directa que dispara trigger ("dell")

DECLARE @RespuestaChatbot NVARCHAR(MAX);
DECLARE @ReglaID INT;

EXEC dbo.SP_ProcesarMensajeChatbot 
    @w_ConversacionID = 1, 
    @w_TextoUsuario = 'Me gustaría ver precios de dell', 
    @o_TextoRespuesta = @RespuestaChatbot OUTPUT,
    @o_ReglaActivadaID = @ReglaID OUTPUT;

SELECT @RespuestaChatbot AS [Bot_Dice], @ReglaID AS [Regla_Disparada];
GO

DECLARE @RespuestaChatbot NVARCHAR(MAX);
DECLARE @ReglaID INT;

EXEC dbo.SP_ProcesarMensajeChatbot 
    @w_ConversacionID = 1, 
    @w_TextoUsuario = 'zapatillas blanco', 
    @o_TextoRespuesta = @RespuestaChatbot OUTPUT,
    @o_ReglaActivadaID = @ReglaID OUTPUT;

SELECT @RespuestaChatbot AS [Bot_Dice], @ReglaID AS [Regla_Disparada];
GO


DECLARE @RespuestaChatbot NVARCHAR(MAX);
DECLARE @ReglaID INT;

EXEC dbo.SP_ProcesarMensajeChatbot 
    @w_ConversacionID = 1, 
    @w_TextoUsuario = 'laptop', 
    @o_TextoRespuesta = @RespuestaChatbot OUTPUT,
    @o_ReglaActivadaID = @ReglaID OUTPUT;

SELECT @RespuestaChatbot AS [Bot_Dice], @ReglaID AS [Regla_Disparada];
GO


-- PRUEBA CASO 3: Búsqueda abierta por defecto (ej: "Zapatillas")

DECLARE @RespuestaChatbot NVARCHAR(MAX);
DECLARE @ReglaID INT;

EXEC dbo.SP_ProcesarMensajeChatbot 
    @w_ConversacionID = 1, 
    @w_TextoUsuario = 'Zapatillas', 
    @o_TextoRespuesta = @RespuestaChatbot OUTPUT,
    @o_ReglaActivadaID = @ReglaID OUTPUT;

SELECT @RespuestaChatbot AS [Bot_Dice], @ReglaID AS [Regla_Disparada];
GO



DECLARE @RespuestaChatbot NVARCHAR(MAX);
DECLARE @ReglaID INT;
-- PRUEBA CASO 3: Búsqueda abierta por defecto (El usuario escribe algo que no es trigger directo, ej: "Zapatillas")
EXEC dbo.SP_ProcesarMensajeChatbot 
    @w_ConversacionID = 1, 
    @w_TextoUsuario = 'perro', 
    @o_TextoRespuesta = @RespuestaChatbot OUTPUT,
    @o_ReglaActivadaID = @ReglaID OUTPUT;

SELECT @RespuestaChatbot AS [Bot_Dice], @ReglaID AS [Regla_Disparada];
