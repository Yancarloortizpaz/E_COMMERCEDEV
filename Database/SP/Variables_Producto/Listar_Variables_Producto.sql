USE [DB_ECOMMERCE]
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductVariables_List]
AS BEGIN
    SELECT productVariableId, productVariableProductId, productVariableValue, productVariablePrice, productVariableCurrencyId, productVariableCreatorId, productVariableCreationDate, productVariableModificatorId, productVariableModificationDate, productVariableStatusId
    FROM [SQM_GENERAL].[Tbl_ProductVariables] (NOLOCK);
END
GO
