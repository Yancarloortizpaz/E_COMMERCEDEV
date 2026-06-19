USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR VARIABLES DE PRODUCTO (Integrando Vista y Optimizado Multi-Búsqueda)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductVariables_Filter]
    @SearchTerm VARCHAR(50) = NULL, 
    @StatusId BIT = NULL
AS BEGIN
    -- Intenta castear el término a INT por si buscan por productVariableId o productId
    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

    SELECT 
        productVariableId,
        productId,
        productName,
        productDescription,
        categoryId,
        categoryName,
        subCategoryId,
        subCategoryName,
        segmentId,
        segmentName,
        markId,
        markName,
        providerId,
        providerName,
        variableValue,
        price,
        currencyId,
        currencyName,
        currencyISO,
        statusId
    FROM [SQM_GENERAL].[VW_PRODUCT_VARIABLES] (NOLOCK)
    WHERE (
        @SearchTerm IS NULL
        OR productVariableId = @SearchId  -- Busca si coincide con el ID de la variable
        OR productId = @SearchId          -- O si coincide con el ID del producto base
        OR productName LIKE '%' + @SearchTerm + '%'
        OR productDescription LIKE '%' + @SearchTerm + '%'
        OR categoryName LIKE '%' + @SearchTerm + '%'
        OR subCategoryName LIKE '%' + @SearchTerm + '%'
        OR segmentName LIKE '%' + @SearchTerm + '%'
        OR markName LIKE '%' + @SearchTerm + '%'
        OR providerName LIKE '%' + @SearchTerm + '%'
        OR variableValue LIKE '%' + @SearchTerm + '%' -- Permite buscar por el valor de la variable (ej. 'XL', 'Rojo')
        OR currencyISO LIKE '%' + @SearchTerm + '%'    -- Permite buscar por moneda (ej. 'USD', 'MXN')
    ) AND (@StatusId IS NULL OR statusId = @StatusId)
    OPTION (RECOMPILE); -- Evita el "Parameter Sniffing" y optimiza según los parámetros enviados
END
GO

EXEC [SQM_GENERAL].[sp_ProductVariables_Filter] 
    @SearchTerm = 'Laptop'

	EXEC [SQM_GENERAL].[sp_ProductVariables_Filter] 
    @SearchTerm = '4'

	EXEC [SQM_GENERAL].[sp_ProductVariables_Filter] 
   
   EXEC [SQM_GENERAL].[sp_ProductVariables_Filter] 
    @SearchTerm = 'femenino'