USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Stocks_GetByProductVariable]
(
    @ProductVariableId INT
)
AS 
BEGIN
    SET NOCOUNT ON;

    SELECT 
        stockId,
        stockProductVariableId,
        stockQuantity,
        stockFactoryDate,
        stockExpirationDate,
        stockCreatorId,
        stockCreationDate,
        stockModificatorId,
        stockModificationDate,
        stockStatusId
    FROM [SQM_GENERAL].[Tbl_Stocks] (NOLOCK)
    WHERE stockProductVariableId = @ProductVariableId
      AND stockStatusId = 1;
END
GO