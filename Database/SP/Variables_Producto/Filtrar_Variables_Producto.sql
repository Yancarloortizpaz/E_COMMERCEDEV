USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductVariables_Filter]
    @ProductId INT = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT productVariableId, productVariableProductId, productVariableValue, productVariablePrice, productVariableCurrencyId, productVariableStatusId
    FROM [SQM_GENERAL].[Tbl_ProductVariables] (NOLOCK)
    WHERE (@ProductId IS NULL OR productVariableProductId = @ProductId) AND (@StatusId IS NULL OR productVariableStatusId = @StatusId);
END
GO
