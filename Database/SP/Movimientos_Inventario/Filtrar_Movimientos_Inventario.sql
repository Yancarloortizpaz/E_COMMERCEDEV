USE [DB_ECOMMERCE]
GO

-- 3. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_StockMovements_Filter]
    @MovementType INT = NULL, @OrderId INT = NULL, @StatusId INT = NULL
AS BEGIN
    SELECT 
        stockMovementId, stockMovementType, stockMovementOrderId, stockMovementReference, 
        stockMovementDate, stockMovementCreationDate, stockMovementStatusId
    FROM [SQM_GENERAL].[Tbl_StockMovements] (NOLOCK)
    WHERE (@MovementType IS NULL OR stockMovementType = @MovementType)
      AND (@OrderId IS NULL OR stockMovementOrderId = @OrderId) 
      AND (@StatusId IS NULL OR stockMovementStatusId = @StatusId);
END
GO
