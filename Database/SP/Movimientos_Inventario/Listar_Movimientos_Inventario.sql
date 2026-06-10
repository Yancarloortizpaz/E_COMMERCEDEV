USE [DB_ECOMMERCE]
GO

-- 2. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_StockMovements_List]
AS BEGIN
    SELECT 
        stockMovementId, stockMovementType, stockMovementOrderId, stockMovementReference, 
        stockMovementDate, stockMovementCreatorId, stockMovementCreationDate, stockMovementStatusId
    FROM [SQM_GENERAL].[Tbl_StockMovements] (NOLOCK);
END
GO
