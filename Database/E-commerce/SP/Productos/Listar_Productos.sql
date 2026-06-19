USE [DB_ECOMMERCE]
GO

-- 4. LISTAR (Integrando Vista)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_List]
AS BEGIN
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
    FROM [SQM_GENERAL].[VW_PRODUCTS] (NOLOCK);
END
GO


exec [SQM_GENERAL].[sp_Products_List]
