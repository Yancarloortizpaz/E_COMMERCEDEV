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
