USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_StockMovements_Create]
    @stockMovementType INT, @stockMovementOrderId INT = NULL, @stockMovementReference NVARCHAR(100) = NULL, 
    @stockMovementDate DATETIME, @stockMovementCreatorId INT, @stockMovementStatusId INT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_StockMovements] 
    (stockMovementType, stockMovementOrderId, stockMovementReference, stockMovementDate, stockMovementCreatorId, stockMovementCreationDate, stockMovementStatusId)
    VALUES 
    (@stockMovementType, @stockMovementOrderId, @stockMovementReference, @stockMovementDate, @stockMovementCreatorId, GETDATE(), @stockMovementStatusId);
END
GO
