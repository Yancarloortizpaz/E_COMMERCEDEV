USE [DB_ECOMMERCE]
GO

-- 1. CREAR CON VALIDACIONES Y PARÁMETROS DE SALIDA
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PaymentOrderDetails_Create]
(
    @orderDetailOrderId INT,
    @orderDetailProductVariableId INT,
    @orderDetailPrice DECIMAL(18,2),
    @orderDetailQuantity INT,
    @orderDetailDiscount DECIMAL(18,2),
    @orderDetailSubTotal DECIMAL(18,2),
    @orderDetailTAX DECIMAL(18,2),
    @orderDetailTotal DECIMAL(18,2),
    @orderDetailCurrencyId INT,
    @orderDetailCreatorId INT,
    @orderDetailStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- Validaciones preliminares
    IF @orderDetailOrderId IS NULL OR @orderDetailOrderId <= 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El ID de la orden (@orderDetailOrderId) es obligatorio.';
        RETURN;
    END;

    IF @orderDetailProductVariableId IS NULL OR @orderDetailProductVariableId <= 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El ID de la variante de producto (@orderDetailProductVariableId) es obligatorio.';
        RETURN;
    END;

    IF @orderDetailPrice IS NULL OR @orderDetailPrice < 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El precio unitario no puede ser negativo.';
        RETURN;
    END;

    IF @orderDetailQuantity IS NULL OR @orderDetailQuantity <= 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'La cantidad del detalle debe ser mayor a cero.';
        RETURN;
    END;

    IF @orderDetailDiscount IS NULL OR @orderDetailDiscount < 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El descuento no puede ser negativo.';
        RETURN;
    END;

    IF @orderDetailSubTotal IS NULL OR @orderDetailTAX IS NULL OR @orderDetailTotal IS NULL
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'Los campos de cálculo (SubTotal, TAX, Total) son obligatorios.';
        RETURN;
    END;

    IF @orderDetailCurrencyId IS NULL OR @orderDetailCurrencyId <= 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'La moneda del detalle es obligatoria.';
        RETURN;
    END;

    IF @orderDetailCreatorId IS NULL OR @orderDetailCreatorId <= 0
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El ID del creador (@orderDetailCreatorId) es obligatorio.';
        RETURN;
    END;

    IF @orderDetailStatusId IS NULL
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El estado del detalle es obligatorio.';
        RETURN;
    END;

    -- Validar existencia de la orden, variante de producto, moneda y creador
    IF NOT EXISTS (SELECT 1 FROM [SQM_GENERAL].[Tbl_PaymentOrders] WHERE orderId = @orderDetailOrderId)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'La orden de pago especificada no existe.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM [SQM_GENERAL].[Tbl_ProductVariables] WHERE productVariableId = @orderDetailProductVariableId AND productVariableStatusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'La variante de producto especificada no existe o se encuentra inactiva.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM [SQM_CATALOGS].[Tbl_Currencies] WHERE currencyId = @orderDetailCurrencyId AND currencyStatusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'La moneda especificada no existe o se encuentra inactiva.';
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM [SQM_SECURITY].[Tbl_Users] WHERE userId = @orderDetailCreatorId AND userStatusId = 1)
    BEGIN
        SET @o_code = -1;
        SET @o_message = 'El usuario creador especificado no existe o se encuentra inactivo.';
        RETURN;
    END;

    -- Bloque transaccional para la inserción
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO [SQM_GENERAL].[Tbl_PaymentOrderDetails]
        (
            orderDetailOrderId,
            orderDetailProductVariableId,
            orderDetailPrice,
            orderDetailQuantity,
            orderDetailDiscount,
            orderDetailSubTotal,
            orderDetailTAX,
            orderDetailTotal,
            orderDetailCurrencyId,
            orderDetailCreatorId,
            orderDetailCreationDate,
            orderDetailStatusId
        )
        VALUES
        (
            @orderDetailOrderId,
            @orderDetailProductVariableId,
            @orderDetailPrice,
            @orderDetailQuantity,
            @orderDetailDiscount,
            @orderDetailSubTotal,
            @orderDetailTAX,
            @orderDetailTotal,
            @orderDetailCurrencyId,
            @orderDetailCreatorId,
            GETDATE(),
            @orderDetailStatusId
        );

        SET @o_templateId = SCOPE_IDENTITY();

        COMMIT TRANSACTION;

        SET @o_code = 200;
        SET @o_message = 'Detalle de orden de pago registrado correctamente.';
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
