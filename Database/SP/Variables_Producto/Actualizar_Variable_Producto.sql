USE [DB_ECOMMERCE]
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductVariables_Update]
    @productVariableId INT, @productVariableProductId INT, @productVariableValue VARCHAR(50), @productVariablePrice DECIMAL(18,2), @productVariableCurrencyId INT, @productVariableModificatorId INT, @productVariableStatusId BIT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_ProductVariables] SET productVariableProductId = @productVariableProductId, productVariableValue = @productVariableValue, productVariablePrice = @productVariablePrice, productVariableCurrencyId = @productVariableCurrencyId, productVariableModificatorId = @productVariableModificatorId, productVariableModificationDate = GETDATE(), productVariableStatusId = @productVariableStatusId
    WHERE productVariableId = @productVariableId;
END
GO
