USE [DB_ECOMMERCE]
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_CartDetails_Update]
    @cartDetailId INT, @cartDetailProductVariableId INT, @cartDetailPrice DECIMAL(18,2), 
    @cartDetailQuantity INT, @cartDetailDiscount DECIMAL(18,2), @cartDetailSubTotal DECIMAL(18,2), 
    @cartDetailTAX DECIMAL(18,2), @cartDetailTotal DECIMAL(18,2), @cartDetailCurrencyId INT, 
    @cartDetailModificatorId INT, @cartDetailStatusId BIT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_CartDetails] 
    SET cartDetailProductVariableId = @cartDetailProductVariableId, cartDetailPrice = @cartDetailPrice, 
        cartDetailQuantity = @cartDetailQuantity, cartDetailDiscount = @cartDetailDiscount, 
        cartDetailSubTotal = @cartDetailSubTotal, cartDetailTAX = @cartDetailTAX, 
        cartDetailTotal = @cartDetailTotal, cartDetailCurrencyId = @cartDetailCurrencyId, 
        cartDetailModificatorId = @cartDetailModificatorId, cartDetailModificationDate = GETDATE(), 
        cartDetailStatusId = @cartDetailStatusId
    WHERE cartDetailId = @cartDetailId;
END
GO
