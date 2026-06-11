USE [DB_ECOMMERCE]
GO

-- 1. CREAR CON VALIDACIONES Y PARÁMETROS DE SALIDA
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_CartDetails_Create]
(
    @cartDetailCartId INT,
    @cartDetailProductVariableId INT,
    @cartDetailPrice DECIMAL(18,2),
    @cartDetailQuantity INT,
    @cartDetailDiscount DECIMAL(18,2),
    @cartDetailSubTotal DECIMAL(18,2),
    @cartDetailTAX DECIMAL(18,2),
    @cartDetailTotal DECIMAL(18,2),
    @cartDetailCurrencyId INT,
    @cartDetailCreatorId INT,
    @cartDetailStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- 1. Validaciones Preliminares de Nulidad y Rango
    IF @cartDetailCartId IS NULL OR @cartDetailCartId <= 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El ID del carrito (@cartDetailCartId) es obligatorio.';
        RETURN;
    END;

    IF @cartDetailProductVariableId IS NULL OR @cartDetailProductVariableId <= 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El ID de la variante de producto (@cartDetailProductVariableId) es obligatorio.';
        RETURN;
    END;

    IF @cartDetailQuantity IS NULL OR @cartDetailQuantity <= 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'La cantidad (@cartDetailQuantity) debe ser mayor a cero.';
        RETURN;
    END;

    IF @cartDetailPrice IS NULL OR @cartDetailPrice < 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El precio (@cartDetailPrice) no puede ser negativo.';
        RETURN;
    END;

    IF @cartDetailDiscount IS NULL OR @cartDetailDiscount < 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El descuento (@cartDetailDiscount) no puede ser negativo.';
        RETURN;
    END;

    IF @cartDetailCurrencyId IS NULL OR @cartDetailCurrencyId <= 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'La moneda (@cartDetailCurrencyId) es obligatoria.';
        RETURN;
    END;

    IF @cartDetailCreatorId IS NULL OR @cartDetailCreatorId <= 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El ID del creador (@cartDetailCreatorId) es obligatorio.';
        RETURN;
    END;

    IF @cartDetailStatusId IS NULL
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El estado del detalle (@cartDetailStatusId) es obligatorio.';
        RETURN;
    END;

    -- 2. Validar existencia del Carrito Activo
    IF NOT EXISTS (SELECT 1 FROM [SQM_GENERAL].[Tbl_Carts] WHERE cartId = @cartDetailCartId AND cartStatusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El carrito especificado no existe o se encuentra inactivo.';
        RETURN;
    END;

    -- 3. Validar existencia de la Variante de Producto Activa
    IF NOT EXISTS (SELECT 1 FROM [SQM_GENERAL].[Tbl_ProductVariables] WHERE productVariableId = @cartDetailProductVariableId AND productVariableStatusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'La variante de producto especificada no existe o se encuentra inactiva.';
        RETURN;
    END;

    -- 4. Validar existencia y estado activo de la Moneda
    IF NOT EXISTS (SELECT 1 FROM [SQM_CATALOGS].[Tbl_Currencies] WHERE currencyId = @cartDetailCurrencyId AND currencyStatusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'La moneda especificada no existe o se encuentra inactiva.';
        RETURN;
    END;

    -- 5. Validar creador activo
    IF NOT EXISTS (SELECT 1 FROM [SQM_SECURITY].[Tbl_Users] WHERE userId = @cartDetailCreatorId AND userStatusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El usuario creador especificado no existe o se encuentra inactivo.';
        RETURN;
    END;

    -- 6. Validación de Inventario (Stock disponible no expirado)
    DECLARE @AvailableStock INT = 0;
    SELECT @AvailableStock = COALESCE(SUM(stockQuantity), 0)
    FROM [SQM_GENERAL].[Tbl_Stocks]
    WHERE stockProductVariableId = @cartDetailProductVariableId 
      AND stockStatusId = 1 
      AND stockExpirationDate >= CAST(GETDATE() AS DATE);

    IF @AvailableStock < @cartDetailQuantity
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'Stock insuficiente. Stock disponible y no expirado: ' + CAST(@AvailableStock AS VARCHAR(10));
        RETURN;
    END;

    -- 7. Validación de Registro Duplicado en el mismo Carrito (Combinar cantidades)
    DECLARE @ExistingDetailId INT;
    DECLARE @ExistingQuantity INT;
    SELECT @ExistingDetailId = cartDetailId, 
           @ExistingQuantity = cartDetailQuantity
    FROM [SQM_GENERAL].[Tbl_CartDetails]
    WHERE cartDetailCartId = @cartDetailCartId 
      AND cartDetailProductVariableId = @cartDetailProductVariableId 
      AND cartDetailStatusId = 1;

    IF @ExistingDetailId IS NOT NULL
    BEGIN
        -- Validar stock sumando las cantidades
        IF @AvailableStock < (@ExistingQuantity + @cartDetailQuantity)
        BEGIN
            SET @o_code = -1;
            SET @o_message = 'Stock insuficiente al acumular la cantidad en el carrito. Stock disponible: ' + CAST(@AvailableStock AS VARCHAR(10));
            RETURN;
        END;

        -- Actualizar acumulado
        BEGIN TRY
            BEGIN TRANSACTION;

            UPDATE [SQM_GENERAL].[Tbl_CartDetails]
            SET cartDetailQuantity = cartDetailQuantity + @cartDetailQuantity,
                cartDetailSubTotal = cartDetailSubTotal + @cartDetailSubTotal,
                cartDetailTAX = cartDetailTAX + @cartDetailTAX,
                cartDetailTotal = cartDetailTotal + @cartDetailTotal,
                cartDetailModificatorId = @cartDetailCreatorId,
                cartDetailModificationDate = GETDATE()
            WHERE cartDetailId = @ExistingDetailId;

            COMMIT TRANSACTION;

            SET @o_code = 200;
            SET @o_message = 'Cantidad de producto acumulada en el detalle de carrito existente.';
            SET @o_templateId = @ExistingDetailId;
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
            SET @o_code = ERROR_NUMBER();
            SET @o_message = ERROR_MESSAGE();
            SET @o_templateId = NULL;
        END CATCH;
        
        RETURN;
    END;

    -- 8. Inserción protegida transaccionalmente
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO [SQM_GENERAL].[Tbl_CartDetails] 
        (
            cartDetailCartId, 
            cartDetailProductVariableId, 
            cartDetailPrice, 
            cartDetailQuantity, 
            cartDetailDiscount, 
            cartDetailSubTotal, 
            cartDetailTAX, 
            cartDetailTotal, 
            cartDetailCurrencyId, 
            cartDetailCreatorId, 
            cartDetailCreationDate, 
            cartDetailStatusId
        )
        VALUES 
        (
            @cartDetailCartId, 
            @cartDetailProductVariableId, 
            @cartDetailPrice, 
            @cartDetailQuantity, 
            @cartDetailDiscount, 
            @cartDetailSubTotal, 
            @cartDetailTAX, 
            @cartDetailTotal, 
            @cartDetailCurrencyId, 
            @cartDetailCreatorId, 
            GETDATE(), 
            @cartDetailStatusId
        );

        SET @o_templateId = SCOPE_IDENTITY();

        COMMIT TRANSACTION;

        SET @o_code = 200;
        SET @o_message = 'Detalle de carrito insertado correctamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @o_code = ERROR_NUMBER();
        SET @o_message = ERROR_MESSAGE();
        SET @o_templateId = NULL;
    END CATCH;
END;
GO

-- ==========================================
-- EJEMPLO DE PRUEBA / EJECUCIÓN
-- ==========================================
/*
DECLARE @v_code INT;
DECLARE @v_message VARCHAR(255);
DECLARE @v_templateId INT;

EXEC [SQM_GENERAL].[sp_CartDetails_Create]
    @cartDetailCartId = 1, -- Asegúrese de usar un carrito activo
    @cartDetailProductVariableId = 1, -- Asegúrese de usar una variante activa y con stock
    @cartDetailPrice = 100.00,
    @cartDetailQuantity = 2,
    @cartDetailDiscount = 0.00,
    @cartDetailSubTotal = 200.00,
    @cartDetailTAX = 30.00,
    @cartDetailTotal = 230.00,
    @cartDetailCurrencyId = 1,
    @cartDetailCreatorId = 1,
    @cartDetailStatusId = 1,
    @o_code = @v_code OUTPUT,
    @o_message = @v_message OUTPUT,
    @o_templateId = @v_templateId OUTPUT;

SELECT 
    @v_code AS CodigoResultado, 
    @v_message AS MensajeResultado, 
    @v_templateId AS CartDetailIdGenerado;
*/
