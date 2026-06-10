USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_CartDetails_Create]
    @cartDetailCartId INT, @cartDetailProductVariableId INT, @cartDetailPrice DECIMAL(18,2), 
    @cartDetailQuantity INT, @cartDetailDiscount DECIMAL(18,2), @cartDetailSubTotal DECIMAL(18,2), 
    @cartDetailTAX DECIMAL(18,2), @cartDetailTotal DECIMAL(18,2), @cartDetailCurrencyId INT, 
    @cartDetailCreatorId INT, @cartDetailStatusId BIT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_CartDetails] 
    (cartDetailCartId, cartDetailProductVariableId, cartDetailPrice, cartDetailQuantity, cartDetailDiscount, cartDetailSubTotal, cartDetailTAX, cartDetailTotal, cartDetailCurrencyId, cartDetailCreatorId, cartDetailCreationDate, cartDetailStatusId)
    VALUES 
    (@cartDetailCartId, @cartDetailProductVariableId, @cartDetailPrice, @cartDetailQuantity, @cartDetailDiscount, @cartDetailSubTotal, @cartDetailTAX, @cartDetailTotal, @cartDetailCurrencyId, @cartDetailCreatorId, GETDATE(), @cartDetailStatusId);
END
GO
