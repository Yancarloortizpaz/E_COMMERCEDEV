USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_StockMovementDetails_Create]
    @stockMovementDetailMovementId INT, @stockMovementDetailOrderDetailId INT = NULL, 
    @stockMovementDetailStockId INT = NULL, @stockMovementDetailQuantity INT, 
    @stockMovementDetailFactoryDate DATE = NULL, @stockMovementDetailExpirationDate DATE = NULL, 
    @stockMovementDetailCreatorId INT, @stockMovementDetailStatusId BIT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_StockMovementDetails] 
    (stockMovementDetailMovementId, stockMovementDetailOrderDetailId, stockMovementDetailStockId, stockMovementDetailQuantity, stockMovementDetailFactoryDate, stockMovementDetailExpirationDate, stockMovementDetailCreatorId, stockMovementDetailCreationDate, stockMovementDetailStatusId)
    VALUES 
    (@stockMovementDetailMovementId, @stockMovementDetailOrderDetailId, @stockMovementDetailStockId, @stockMovementDetailQuantity, @stockMovementDetailFactoryDate, @stockMovementDetailExpirationDate, @stockMovementDetailCreatorId, GETDATE(), @stockMovementDetailStatusId);
END
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