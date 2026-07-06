USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_Filter]
    @SearchTerm VARCHAR(50) = NULL
AS BEGIN
    SET NOCOUNT ON;

    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

    SELECT 
        productId,
        productName,
        productDescription,
        productIdentificatorId,
        categoryId,
        categoryName,
        subCategoryId,
        subCategoryName,
        segmentId,
        segmentName,
        markByProviderId,
        markId,
        markName,
        providerId,
        providerName,
        statusId
    FROM [SQM_GENERAL].[VW_PRODUCTS] (NOLOCK)
    WHERE (
        @SearchTerm IS NULL
        OR productId = @SearchId
        OR productName LIKE '%' + @SearchTerm + '%'
        OR productDescription LIKE '%' + @SearchTerm + '%'
        OR categoryName LIKE '%' + @SearchTerm + '%'
        OR subCategoryName LIKE '%' + @SearchTerm + '%'
        OR markName LIKE '%' + @SearchTerm + '%'
        OR providerName LIKE '%' + @SearchTerm + '%'
    ) 
    AND statusId = 1
    OPTION (RECOMPILE);
END
GO

-- Caso 1: Sin parámetros (Debería traer todo el universo de productos sin restricciones)
EXEC [SQM_GENERAL].[sp_Products_Filter];
GO

-- Caso 2: Filtrar solo por ID de producto (El SP convertirá internamente el texto a INT)
EXEC [SQM_GENERAL].[sp_Products_Filter] 
    @SearchTerm = '1';
GO

-- Caso 3: Filtrar solo por coincidencia de texto (Aplica para Nombre, Categoría, Marca, Proveedor, etc.)
EXEC [SQM_GENERAL].[sp_Products_Filter] 
    @SearchTerm = 'Laptop';
GO

