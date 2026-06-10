USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductVariables_Create]
    @productVariableProductId INT, @productVariableValue VARCHAR(50), @productVariablePrice DECIMAL(18,2), @productVariableCurrencyId INT, @productVariableCreatorId INT, @productVariableStatusId BIT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_ProductVariables] (productVariableProductId, productVariableValue, productVariablePrice, productVariableCurrencyId, productVariableCreatorId, productVariableCreationDate, productVariableStatusId)
    VALUES (@productVariableProductId, @productVariableValue, @productVariablePrice, @productVariableCurrencyId, @productVariableCreatorId, GETDATE(), @productVariableStatusId);
END
GO
