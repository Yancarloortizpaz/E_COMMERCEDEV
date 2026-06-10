USE [DB_ECOMMERCE]
GO

-- 3. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Stocks_List]
AS BEGIN
    SELECT 
        stockId, stockProductVariableId, stockQuantity, stockFactoryDate, 
        stockExpirationDate, stockCreatorId, stockCreationDate, 
        stockModificatorId, stockModificationDate, stockStatusId
    FROM [SQM_GENERAL].[Tbl_Stocks] (NOLOCK);
END
GO
