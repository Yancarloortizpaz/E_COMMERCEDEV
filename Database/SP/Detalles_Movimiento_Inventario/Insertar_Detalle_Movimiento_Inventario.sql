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
