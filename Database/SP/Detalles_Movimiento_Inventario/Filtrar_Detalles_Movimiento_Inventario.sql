USE [DB_ECOMMERCE]
GO

-- 3. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_StockMovementDetails_Filter]
    @MovementId INT = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT 
        stockMovementDetailId, stockMovementDetailMovementId, stockMovementDetailOrderDetailId, 
        stockMovementDetailStockId, stockMovementDetailQuantity, stockMovementDetailFactoryDate, 
        stockMovementDetailExpirationDate, stockMovementDetailCreationDate, stockMovementDetailStatusId
    FROM [SQM_GENERAL].[Tbl_StockMovementDetails] (NOLOCK)
    WHERE (@MovementId IS NULL OR stockMovementDetailMovementId = @MovementId)
      AND (@StatusId IS NULL OR stockMovementDetailStatusId = @StatusId);
END
GO
