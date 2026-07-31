USE [DB_EcommerceAgent]
GO


CREATE OR ALTER PROCEDURE dbo.SP_ProcesarMensajeChatbot
    @w_ConversacionID VARCHAR(50),
    @w_TextoUsuario VARCHAR(1000),
    @o_TextoRespuesta NVARCHAR(MAX) OUTPUT,
    @o_ReglaActivadaID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. REGISTRAR O RECUPERAR LA CONVERSACIÓN Y EL MENSAJE ENTRANTE DEL USUARIO
    IF NOT EXISTS (SELECT 1 FROM Conversaciones WHERE ConversacionID = CAST(@w_ConversacionID AS VARCHAR(50)))
    BEGIN
        INSERT INTO Conversaciones (ConversacionID, UsuarioID, FechaInicio, Activo)
        VALUES (CAST(@w_ConversacionID AS VARCHAR(50)), '1', GETDATE(), 1); -- Default UsuarioID = '1'
    END

    INSERT INTO Mensajes (ConversacionID, Rol, ChatBot, Contenido, FechaHora, ReglaActivadaID)
    VALUES (CAST(@w_ConversacionID AS VARCHAR(50)), 'user', 0, @w_TextoUsuario, GETDATE(), NULL);

    -- Obtener UsuarioID numérico asociado a la conversación (default 1)
    DECLARE @v_UserId INT = 1;
    SELECT @v_UserId = COALESCE(TRY_CAST(UsuarioID AS INT), 1)
    FROM Conversaciones
    WHERE ConversacionID = CAST(@w_ConversacionID AS VARCHAR(50));

    DECLARE @v_ReglaID INT = NULL;
    DECLARE @v_PlantillaTexto NVARCHAR(MAX) = '';
    DECLARE @v_FiltroTexto VARCHAR(100) = NULL;
    DECLARE @v_EsBusquedaPorDescarte BIT = 0;
    DECLARE @w_TextoLower VARCHAR(1000) = LOWER(TRIM(@w_TextoUsuario));

    -- 2. EVALUAR SI EL TEXTO CONTIENE UN TRIGGER DE PALABRA CLAVE
    SELECT TOP 1 
        @v_ReglaID = ReglaID
    FROM PalabrasClaveRegla
    WHERE Activo = 1
      AND CHARINDEX(LOWER(PalabraClave), @w_TextoLower) > 0
    ORDER BY LEN(PalabraClave) DESC; -- Preferir coincidencias de frases más largas

    -- 3. INVERSIÓN DE LA REGLA: Si no coincide con ningún trigger fijo, se asume Buscar Producto (Regla 2)
    IF @v_ReglaID IS NULL
    BEGIN
        SET @v_ReglaID = 2; 
        SET @v_EsBusquedaPorDescarte = 1;
    END

    SET @v_FiltroTexto = TRIM(@w_TextoUsuario);
    SET @o_ReglaActivadaID = @v_ReglaID;

    -- 4. HELPER DENTRO DEL SP PARA EXTRAER NÚMEROS DEL TEXTO (Cantidad / IDs)
    DECLARE @v_FirstNum INT = NULL;
    DECLARE @v_SecondNum INT = NULL;

    -- Limpiar espacios dobles o múltiples a un solo espacio
    DECLARE @v_TextClean VARCHAR(1000) = TRIM(@w_TextoUsuario);
    WHILE CHARINDEX('  ', @v_TextClean) > 0
        SET @v_TextClean = REPLACE(@v_TextClean, '  ', ' ');

    SET @v_TextClean = @v_TextClean + ' '; -- Espacio final de seguridad
    DECLARE @v_Pos INT = 1;
    DECLARE @v_CurrentNum VARCHAR(50) = '';

    WHILE @v_Pos <= DATALENGTH(@v_TextClean)
    BEGIN
        IF SUBSTRING(@v_TextClean, @v_Pos, 1) LIKE '[0-9]'
        BEGIN
            SET @v_CurrentNum = @v_CurrentNum + SUBSTRING(@v_TextClean, @v_Pos, 1);
        END
        ELSE
        BEGIN
            IF LEN(@v_CurrentNum) > 0
            BEGIN
                IF @v_FirstNum IS NULL
                    SET @v_FirstNum = TRY_CAST(@v_CurrentNum AS INT);
                ELSE IF @v_SecondNum IS NULL
                    SET @v_SecondNum = TRY_CAST(@v_CurrentNum AS INT);
                
                SET @v_CurrentNum = '';
            END
        END
        SET @v_Pos = @v_Pos + 1;
    END;

    -- 5. CONTROL DE FLUJO SEGÚN REGLA ACTIVADA

    
    -- REGLA 2: BUSCAR PRODUCTO
    
    IF @v_ReglaID = 2
    BEGIN
        DECLARE @CoincidenceScore TABLE (
            ProductID INT, ProductName VARCHAR(50), ProductVariableID INT, ProductVariableName VARCHAR(50),
            ProductVariablePrice DECIMAL(18,2), CurrencyID INT, CurrencyISO CHAR(5), CategoryID INT,
            CategoryName VARCHAR(50), SubcategoryID INT, SubcategoryName VARCHAR(50), SegmentID INT,
            SegmentName VARCHAR(50), MarkID INT, MarkName VARCHAR(50), ProviderID INT,
            ProviderName VARCHAR(50), StockID INT, StockAvilable INT, StockFactoryDate DATE, StockExpirationDate DATE,
            CoincidenceScore INT
        );

        INSERT INTO @CoincidenceScore
        EXEC dbo.SP_ListarGeneralProducts_Filtro @01_FilterText = @v_FiltroTexto;

        IF EXISTS (SELECT 1 FROM @CoincidenceScore WHERE StockAvilable > 0)
        BEGIN
            SELECT TOP 1 @v_PlantillaTexto = TextoRespuesta FROM PlantillasRespuesta WHERE ReglaID = 2 AND Activo = 1 ORDER BY NEWID();

            DECLARE @v_ListaFormateada NVARCHAR(MAX) = '';
            SELECT @v_ListaFormateada = @v_ListaFormateada + 
                CHAR(13) + CHAR(10) + '- [ID: ' + CAST(ProductVariableID AS VARCHAR) + '] ' + ProductName + ' (' + ProductVariableName + ') | Precio: ' + 
                CurrencyISO + ' ' + CAST(ProductVariablePrice AS VARCHAR) + ' | Stock: ' + CAST(StockAvilable AS VARCHAR)
            FROM @CoincidenceScore
            WHERE StockAvilable > 0;

            IF @v_PlantillaTexto IS NULL OR @v_PlantillaTexto = ''
                SET @v_PlantillaTexto = 'Encontré estas opciones disponibles en nuestro catálogo:' + CHAR(13) + CHAR(10) + '[@TABLA]';

            SET @o_TextoRespuesta = REPLACE(@v_PlantillaTexto, '[@TABLA]', @v_ListaFormateada);
        END
        ELSE
        BEGIN
            IF @v_EsBusquedaPorDescarte = 1 AND LEN(@v_FiltroTexto) < 4
            BEGIN
                SELECT TOP 1 @v_PlantillaTexto = TextoRespuesta FROM PlantillasRespuesta WHERE ReglaID = 6 AND Activo = 1 ORDER BY NEWID();
                SET @o_TextoRespuesta = ISNULL(@v_PlantillaTexto, 'Lo siento, no logré entender tu consulta.');
                SET @o_ReglaActivadaID = 6;
            END
            ELSE
            BEGIN
                SELECT TOP 1 @v_PlantillaTexto = TextoRespuesta FROM PlantillasRespuesta WHERE ReglaID = 5 AND Activo = 1 ORDER BY NEWID();
                SET @o_TextoRespuesta = ISNULL(@v_PlantillaTexto, 'No encontramos productos que coincidan con tu búsqueda.');
                SET @o_ReglaActivadaID = 5;
            END
        END
    END

    
    -- REGLA 8: AGREGAR CARRITO
    
    ELSE IF @v_ReglaID = 8
    BEGIN
        DECLARE @v_ProdVarId INT = NULL;
        DECLARE @v_Cantidad INT = 1;

        IF @v_SecondNum IS NOT NULL
        BEGIN
            SET @v_Cantidad = @v_FirstNum;
            SET @v_ProdVarId = @v_SecondNum;
        END
        ELSE IF @v_FirstNum IS NOT NULL
        BEGIN
            SET @v_Cantidad = 1;
            SET @v_ProdVarId = @v_FirstNum;
        END;

        IF @v_ProdVarId IS NULL
        BEGIN
            SET @o_TextoRespuesta = 'Para agregar un producto al carrito, por favor indica el ID del producto (ejemplo: "agregar 2 del producto 5" o "agregar producto 3").';
        END
        ELSE
        BEGIN
            DECLARE @v_Code INT, @v_Message VARCHAR(255), @v_TemplateId INT;

            EXEC [DB_ECOMMERCE].[SQM_GENERAL].[sp_CartDetails_Create]
                @userId = @v_UserId,
                @productVariableId = @v_ProdVarId,
                @quantity = @v_Cantidad,
                @discount = 0.00,
                @creatorId = @v_UserId,
                @statusId = 1,
                @o_code = @v_Code OUTPUT,
                @o_message = @v_Message OUTPUT,
                @o_templateId = @v_TemplateId OUTPUT;

            IF @v_Code = 200
            BEGIN
                DECLARE @v_NombreProducto VARCHAR(100) = '';
                SELECT TOP 1 @v_NombreProducto = P.productName + ' (' + PV.productVariableValue + ')'
                FROM [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_ProductVariables] PV
                INNER JOIN [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_Products] P ON PV.productVariableProductId = P.productId
                WHERE PV.productVariableId = @v_ProdVarId;

                SET @o_TextoRespuesta = ' ¡Producto agregado al carrito exitosamente!' + CHAR(13) + CHAR(10) +
                                        '- Producto: ' + ISNULL(@v_NombreProducto, 'ID ' + CAST(@v_ProdVarId AS VARCHAR)) + CHAR(13) + CHAR(10) +
                                        '- Cantidad: ' + CAST(@v_Cantidad AS VARCHAR) + CHAR(13) + CHAR(10) +
                                        'Escribe "ver carrito" para revisar tus productos o "procesar pago" para finalizar.';
            END
            ELSE
            BEGIN
                SET @o_TextoRespuesta = ' No se pudo agregar al carrito: ' + ISNULL(@v_Message, 'Error desconocido');
            END
        END
    END

    
    -- REGLA 9: CONSULTAR CARRITO
    
    ELSE IF @v_ReglaID = 9
    BEGIN
        DECLARE @v_CartIdCons INT = NULL;
        SELECT @v_CartIdCons = cartId 
        FROM [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_Carts] 
        WHERE cartUserId = @v_UserId AND cartStatusId = 1;

        IF @v_CartIdCons IS NULL OR NOT EXISTS (
            SELECT 1 FROM [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_CartDetails] 
            WHERE cartDetailCartId = @v_CartIdCons AND cartDetailStatusId = 1
        )
        BEGIN
            SET @o_TextoRespuesta = ' Tu carrito de compras está actualmente vacío. ¡Agrega productos buscando en nuestro catálogo!';
        END
        ELSE
        BEGIN
            DECLARE @v_TablaCarrito NVARCHAR(MAX) = '';
            DECLARE @v_TotalCarrito DECIMAL(18,2) = 0;

            SELECT 
                @v_TablaCarrito = @v_TablaCarrito + CHAR(13) + CHAR(10) + 
                    '- [ID: ' + CAST(PV.productVariableId AS VARCHAR) + '] ' + P.productName + ' (' + PV.productVariableValue + ') | Cantidad: ' + CAST(CD.cartDetailQuantity AS VARCHAR) + 
                    ' | Precio: ' + CAST(CD.cartDetailPrice AS VARCHAR) + ' | SubTotal: ' + CAST(CD.cartDetailTotal AS VARCHAR),
                @v_TotalCarrito = @v_TotalCarrito + CD.cartDetailTotal
            FROM [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_CartDetails] CD
            INNER JOIN [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_ProductVariables] PV ON CD.cartDetailProductVariableId = PV.productVariableId
            INNER JOIN [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_Products] P ON PV.productVariableProductId = P.productId
            WHERE CD.cartDetailCartId = @v_CartIdCons AND CD.cartDetailStatusId = 1;

            SELECT TOP 1 @v_PlantillaTexto = TextoRespuesta FROM PlantillasRespuesta WHERE ReglaID = 9 AND Activo = 1 ORDER BY NEWID();
            IF @v_PlantillaTexto IS NULL OR @v_PlantillaTexto = ''
                SET @v_PlantillaTexto = ' Aquí tienes los productos en tu carrito:' + CHAR(13) + CHAR(10) + '[@TABLA_CARRITO]' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) + '💰 Total a pagar: C$ [@TOTAL_CARRITO]';

            SET @o_TextoRespuesta = REPLACE(REPLACE(@v_PlantillaTexto, '[@TABLA_CARRITO]', @v_TablaCarrito), '[@TOTAL_CARRITO]', CAST(@v_TotalCarrito AS VARCHAR));
        END
    END

    
    -- REGLA 10: ELIMINAR PRODUCTO CARRITO (O VACIAR CARRITO)
    
    ELSE IF @v_ReglaID = 10
    BEGIN
        DECLARE @v_CartIdDel INT = NULL;
        SELECT @v_CartIdDel = cartId 
        FROM [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_Carts] 
        WHERE cartUserId = @v_UserId AND cartStatusId = 1;

        IF @v_CartIdDel IS NULL OR NOT EXISTS (
            SELECT 1 FROM [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_CartDetails] 
            WHERE cartDetailCartId = @v_CartIdDel AND cartDetailStatusId = 1
        )
        BEGIN
            SET @o_TextoRespuesta = 'No tienes productos en tu carrito activo para eliminar.';
        END
        ELSE IF @w_TextoLower LIKE '%vaciar%' OR @w_TextoLower LIKE '%limpiar%' OR @w_TextoLower LIKE '%borrar todo%' OR @w_TextoLower LIKE '%eliminar todo%'
        BEGIN
            UPDATE [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_CartDetails]
            SET cartDetailStatusId = 0, cartDetailModificationDate = GETDATE()
            WHERE cartDetailCartId = @v_CartIdDel AND cartDetailStatusId = 1;

            SET @o_TextoRespuesta = ' Tu carrito de compras ha sido vaciado completamente.';
        END
        ELSE
        BEGIN
            DECLARE @v_TargetProdId INT = @v_FirstNum;

            IF @v_TargetProdId IS NULL
            BEGIN
                SET @o_TextoRespuesta = 'Para eliminar un producto, indica el ID del producto (ej: "eliminar producto 5") o escribe "vaciar carrito" para removerlo todo.';
            END
            ELSE
            BEGIN
                DECLARE @v_CartDetailId INT = NULL;
                
                SELECT TOP 1 @v_CartDetailId = cartDetailId
                FROM [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_CartDetails]
                WHERE cartDetailCartId = @v_CartIdDel 
                  AND cartDetailStatusId = 1
                  AND (cartDetailProductVariableId = @v_TargetProdId OR cartDetailId = @v_TargetProdId);

                IF @v_CartDetailId IS NULL
                BEGIN
                    SET @o_TextoRespuesta = 'No se encontró el producto ID ' + CAST(@v_TargetProdId AS VARCHAR) + ' en tu carrito activo.';
                END
                ELSE
                BEGIN
                    DECLARE @v_CodeDel INT, @v_MsgDel VARCHAR(255), @v_TplDel INT;

                    EXEC [DB_ECOMMERCE].[SQM_GENERAL].[sp_CartDetails_Delete]
                        @cartDetailId = @v_CartDetailId,
                        @cartDetailModificatorId = @v_UserId,
                        @o_code = @v_CodeDel OUTPUT,
                        @o_message = @v_MsgDel OUTPUT,
                        @o_templateId = @v_TplDel OUTPUT;

                    IF @v_CodeDel = 200
                        SET @o_TextoRespuesta = ' El producto ha sido eliminado de tu carrito exitosamente.';
                    ELSE
                        SET @o_TextoRespuesta = ' No se pudo eliminar el producto: ' + ISNULL(@v_MsgDel, 'Error desconocido');
                END
            END
        END
    END

    
    -- REGLA 11: PROCESAR PAGO
    
    ELSE IF @v_ReglaID = 11
    BEGIN
        DECLARE @v_CartIdPay INT = NULL;
        SELECT @v_CartIdPay = cartId 
        FROM [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_Carts] 
        WHERE cartUserId = @v_UserId AND cartStatusId = 1;

        IF @v_CartIdPay IS NULL OR NOT EXISTS (
            SELECT 1 FROM [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_CartDetails] 
            WHERE cartDetailCartId = @v_CartIdPay AND cartDetailStatusId = 1
        )
        BEGIN
            SET @o_TextoRespuesta = ' Tu carrito está vacío. Agrega productos antes de procesar el pago.';
        END
        ELSE
        BEGIN
            DECLARE @v_AddrId INT = NULL;
            SELECT TOP 1 @v_AddrId = userAddressId 
            FROM [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_UserAddress] 
            WHERE userAddressUserId = @v_UserId AND userAddressStatusId = 1
            ORDER BY userAddressId ASC;

            DECLARE @v_PayMethodId INT = NULL;
            SELECT TOP 1 @v_PayMethodId = userPaymentMethodId 
            FROM [DB_ECOMMERCE].[SQM_GENERAL].[Tbl_UserPaymentMethods] 
            WHERE userPaymentMethodUserId = @v_UserId AND userPaymentMethodStatusId = 1
            ORDER BY userPaymentMethodId ASC;

            IF @v_AddrId IS NULL
            BEGIN
                SET @o_TextoRespuesta = '📍 No tienes una dirección de entrega activa registrada en tu usuario. Por favor agrega una dirección para finalizar tu pedido.';
            END
            ELSE IF @v_PayMethodId IS NULL
            BEGIN
                SET @o_TextoRespuesta = '💳 No tienes un método de pago activo registrado en tu usuario. Por favor agrega un método de pago para finalizar tu pedido.';
            END
            ELSE
            BEGIN
                DECLARE @v_CodePay INT, @v_MsgPay VARCHAR(255), @v_OrderIdGenerado INT;

                EXEC [DB_ECOMMERCE].[SQM_GENERAL].[sp_PaymentOrders_Create]
                    @orderUserId = @v_UserId,
                    @orderDeliveryAddress = @v_AddrId,
                    @orderPaymentMethodId = @v_PayMethodId,
                    @orderSubtotal = NULL,
                    @orderDiscount = NULL,
                    @orderShipping = 50.00,
                    @orderTAX = NULL,
                    @orderTotal = NULL,
                    @orderCurrencyId = 1,
                    @orderCreatorId = @v_UserId,
                    @orderStatusId = 1,
                    @o_code = @v_CodePay OUTPUT,
                    @o_message = @v_MsgPay OUTPUT,
                    @o_templateId = @v_OrderIdGenerado OUTPUT;

                IF @v_CodePay = 200
                BEGIN
                    SELECT TOP 1 @v_PlantillaTexto = TextoRespuesta FROM PlantillasRespuesta WHERE ReglaID = 11 AND Activo = 1 ORDER BY NEWID();
                    IF @v_PlantillaTexto IS NULL OR @v_PlantillaTexto = ''
                        SET @v_PlantillaTexto = '🎉 ¡Pago procesado con éxito! Se ha generado tu Orden de Compra #[@ORDEN_ID]. ¡Gracias por elegirnos!';

                    SET @o_TextoRespuesta = REPLACE(@v_PlantillaTexto, '[@ORDEN_ID]', CAST(@v_OrderIdGenerado AS VARCHAR));
                END
                ELSE
                BEGIN
                    SET @o_TextoRespuesta = ' No se pudo procesar el pago: ' + ISNULL(@v_MsgPay, 'Error en el checkout.');
                END
            END
        END
    END

    
    -- REGLA 12: CONSULTAR ORDEN
    
    ELSE IF @v_ReglaID = 12
    BEGIN
        DECLARE @v_OrdId INT = NULL, @v_OrdStatus VARCHAR(50) = NULL, @v_OrdTotal DECIMAL(18,2) = NULL, @v_OrdDate DATETIME = NULL, @v_OrdCurrency CHAR(5) = NULL;

        SELECT TOP 1 
            @v_OrdId = orderId,
            @v_OrdStatus = statusName,
            @v_OrdTotal = total,
            @v_OrdDate = creationDate,
            @v_OrdCurrency = currencyISO
        FROM [DB_ECOMMERCE].[SQM_GENERAL].[VW_PAYMENT_ORDERS]
        WHERE userId = @v_UserId
        ORDER BY orderId DESC;

        IF @v_OrdId IS NULL
        BEGIN
            SET @o_TextoRespuesta = '📦 No hemos encontrado órdenes registradas para tu usuario.';
        END
        ELSE
        BEGIN
            SELECT TOP 1 @v_PlantillaTexto = TextoRespuesta FROM PlantillasRespuesta WHERE ReglaID = 12 AND Activo = 1 ORDER BY NEWID();
            IF @v_PlantillaTexto IS NULL OR @v_PlantillaTexto = ''
                SET @v_PlantillaTexto = '📦 Tu última orden es la #[@ORDEN_ID] del [@FECHA_ORDEN]. Estado: [@ESTADO_ORDEN] | Total: [@CURRENCY] [@TOTAL_ORDEN].';

            SET @o_TextoRespuesta = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
                @v_PlantillaTexto, 
                '[@ORDEN_ID]', CAST(@v_OrdId AS VARCHAR)),
                '[@FECHA_ORDEN]', CONVERT(VARCHAR, @v_OrdDate, 103)),
                '[@ESTADO_ORDEN]', ISNULL(@v_OrdStatus, 'Procesando')),
                '[@CURRENCY]', ISNULL(@v_OrdCurrency, 'NIO')),
                '[@TOTAL_ORDEN]', CAST(@v_OrdTotal AS VARCHAR));
        END
    END

    
    -- FLUJO DE CONTROL ESTÁTICO (Saludos, Métodos de Pago, Despedidas, etc.)
    
    ELSE
    BEGIN
        SELECT TOP 1 @v_PlantillaTexto = TextoRespuesta FROM PlantillasRespuesta WHERE ReglaID = @v_ReglaID AND Activo = 1 ORDER BY NEWID();
        SET @o_TextoRespuesta = ISNULL(@v_PlantillaTexto, 'Gracias por comunicarte con nosotros.');
    END

    -- 6. REGISTRAR LA RESPUESTA FINAL EMITIDA POR EL BOT
    INSERT INTO Mensajes (ConversacionID, Rol, ChatBot, Contenido, FechaHora, ReglaActivadaID)
    VALUES (CAST(@w_ConversacionID AS VARCHAR(50)), 'assistant', 1, LEFT(@o_TextoRespuesta, 1000), GETDATE(), @o_ReglaActivadaID);

END
GO

