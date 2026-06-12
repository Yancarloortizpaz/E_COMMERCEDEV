USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_StockMovementDetails_Filter]
    @MovementId INT = NULL,
    @SearchTerm VARCHAR(50) = NULL
AS BEGIN
    -- Intentamos convertir el texto de búsqueda a Fecha de manera segura
    DECLARE @SearchDate DATETIME = TRY_CAST(@SearchTerm AS DATETIME);

    SELECT 
        stockMovementDetailId, 
        stockMovementDetailMovementId, 
        stockMovementDetailOrderDetailId, 
        stockMovementDetailStockId, 
        stockMovementDetailQuantity, 
        stockMovementDetailFactoryDate, 
        stockMovementDetailExpirationDate, 
        stockMovementDetailCreationDate, 
        stockMovementDetailStatusId
    FROM [SQM_GENERAL].[Tbl_StockMovementDetails] (NOLOCK)
    WHERE 
        -- 1. Filtro estricto por ID de Movimiento Cabecera
        (@MovementId IS NULL OR stockMovementDetailMovementId = @MovementId)
        
        -- 2. Buscador Universal Inteligente (Texto, IDs, Fechas y Estados)
        AND (
            @SearchTerm IS NULL
            OR stockMovementDetailOrderDetailId LIKE '%' + @SearchTerm + '%'
            OR stockMovementDetailStockId LIKE '%' + @SearchTerm + '%'
            OR stockMovementDetailStatusId LIKE '%' + @SearchTerm + '%' 
            OR (@SearchDate IS NOT NULL AND (
                -- Filtra ignorando las horas, comparando solo Año-Mes-Día
                CONVERT(DATE, stockMovementDetailFactoryDate) = CONVERT(DATE, @SearchDate)
                OR CONVERT(DATE, stockMovementDetailExpirationDate) = CONVERT(DATE, @SearchDate)
                OR CONVERT(DATE, stockMovementDetailCreationDate) = CONVERT(DATE, @SearchDate)
            ))
        )
        
    OPTION (RECOMPILE);
END
GO

exec [SQM_GENERAL].[sp_StockMovementDetails_Filter]