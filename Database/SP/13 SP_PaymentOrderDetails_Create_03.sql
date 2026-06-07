USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PaymentOrderDetails_Create]
    @orderDetailOrderId INT, @orderDetailProductVariableId INT, @orderDetailPrice DECIMAL(18,2), 
    @orderDetailQuantity INT, @orderDetailDiscount DECIMAL(18,2), @orderDetailSubTotal DECIMAL(18,2), 
    @orderDetailTAX DECIMAL(18,2), @orderDetailTotal DECIMAL(18,2), @orderDetailCurrencyId INT, 
    @orderDetailCreatorId INT, @orderDetailStatusId BIT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_PaymentOrderDetails] 
    (orderDetailOrderId, orderDetailProductVariableId, orderDetailPrice, orderDetailQuantity, orderDetailDiscount, orderDetailSubTotal, orderDetailTAX, orderDetailTotal, orderDetailCurrencyId, orderDetailCreatorId, orderDetailCreationDate, orderDetailStatusId)
    VALUES 
    (@orderDetailOrderId, @orderDetailProductVariableId, @orderDetailPrice, @orderDetailQuantity, @orderDetailDiscount, @orderDetailSubTotal, @orderDetailTAX, @orderDetailTotal, @orderDetailCurrencyId, @orderDetailCreatorId, GETDATE(), @orderDetailStatusId);
END
GO

-- 2. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PaymentOrderDetails_List]
AS BEGIN
    SELECT 
        orderDetailId, orderDetailOrderId, orderDetailProductVariableId, orderDetailPrice, 
        orderDetailQuantity, orderDetailDiscount, orderDetailSubTotal, orderDetailTAX, 
        orderDetailTotal, orderDetailCurrencyId, orderDetailCreatorId, orderDetailCreationDate, 
        orderDetailStatusId
    FROM [SQM_GENERAL].[Tbl_PaymentOrderDetails] (NOLOCK);
END
GO

-- 3. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PaymentOrderDetails_Filter]
    @OrderId INT = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT 
        orderDetailId, orderDetailOrderId, orderDetailProductVariableId, orderDetailPrice, 
        orderDetailQuantity, orderDetailDiscount, orderDetailSubTotal, orderDetailTAX, 
        orderDetailTotal, orderDetailCurrencyId, orderDetailCreationDate, orderDetailStatusId
    FROM [SQM_GENERAL].[Tbl_PaymentOrderDetails] (NOLOCK)
    WHERE (@OrderId IS NULL OR orderDetailOrderId = @OrderId) 
      AND (@StatusId IS NULL OR orderDetailStatusId = @StatusId);
END
GO