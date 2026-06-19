USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_StockMovements_Filter]
    @SearchTerm VARCHAR(50) = NULL
AS BEGIN
    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);
    DECLARE @SearchDate DATETIME = TRY_CAST(@SearchTerm AS DATETIME);

    SELECT 
        stockMovementId, 
        stockMovementType, 
        stockMovementOrderId, 
        stockMovementReference, 
        stockMovementDate, 
        stockMovementCreationDate, 
        stockMovementStatusId
    FROM [SQM_GENERAL].[Tbl_StockMovements] (NOLOCK)
    WHERE 
        @SearchTerm IS NULL
        OR stockMovementId = @SearchId
        OR stockMovementType = @SearchId
        OR stockMovementOrderId = @SearchId
        OR stockMovementStatusId = @SearchId
        OR stockMovementReference LIKE '%' + @SearchTerm + '%'
        OR (@SearchDate IS NOT NULL AND (
            CONVERT(DATE, stockMovementDate) = CONVERT(DATE, @SearchDate)
            OR CONVERT(DATE, stockMovementCreationDate) = CONVERT(DATE, @SearchDate)
        ))
    OPTION (RECOMPILE);
END
GO

EXEC [SQM_GENERAL].[sp_StockMovements_Filter];
GO