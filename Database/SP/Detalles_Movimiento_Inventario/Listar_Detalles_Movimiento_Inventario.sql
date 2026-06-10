USE [DB_ECOMMERCE]
GO

-- 2. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_StockMovementDetails_List]
AS BEGIN
    SELECT 
        stockMovementDetailId, stockMovementDetailMovementId, stockMovementDetailOrderDetailId, 
        stockMovementDetailStockId, stockMovementDetailQuantity, stockMovementDetailFactoryDate, 
        stockMovementDetailExpirationDate, stockMovementDetailCreatorId, stockMovementDetailCreationDate, 
        stockMovementDetailStatusId
    FROM [SQM_GENERAL].[Tbl_StockMovementDetails] (NOLOCK);
END
GO
