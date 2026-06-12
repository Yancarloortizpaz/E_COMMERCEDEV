USE [DB_ECOMMERCE]
GO

-- 4. LISTAR (Integrando Vista)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductVariables_List]
AS BEGIN
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
    FROM [SQM_GENERAL].[VW_PRODUCT_VARIABLES] (NOLOCK);
END
GO
