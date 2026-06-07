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

-- 2. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_StockMovements_List]
AS BEGIN
    SELECT 
        stockMovementId, stockMovementType, stockMovementOrderId, stockMovementReference, 
        stockMovementDate, stockMovementCreatorId, stockMovementCreationDate, stockMovementStatusId
    FROM [SQM_GENERAL].[Tbl_StockMovements] (NOLOCK);
END
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