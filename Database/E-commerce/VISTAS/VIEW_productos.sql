USE [DB_ECOMMERCE]
GO

CREATE OR ALTER VIEW [SQM_GENERAL].[VW_PRODUCTS]
AS
SELECT 
    [P].[productId],
    [P].[productName],
    [P].[productDescription],
    [P].[productProductIdentificatorId] AS [productIdentificatorId],
    [PI].[categoryId],
    [PI].[categoryName],
    [PI].[subCategoryId],
    [PI].[subCategoryName],
    [PI].[segmentId],
    [PI].[segmentName],
    [P].[productMarkByProviderId] AS [markByProviderId],
    [MP].[markId],
    [MP].[markName],
    [MP].[providerId],
    [MP].[providerName],
    [P].[productStatusId] AS [statusId]
FROM [SQM_GENERAL].[Tbl_Products] [P]
INNER JOIN [SQM_CATALOGS].[VW_PRODUCT_IDENTIFICATORS] [PI] ON [P].[productProductIdentificatorId] = [PI].[productIdentificatorId]
INNER JOIN [SQM_CATALOGS].[VW_MARKS_BY_PROVIDER] [MP] ON [P].[productMarkByProviderId] = [MP].[markByProviderId]
WHERE [P].[productStatusId] = 1;
GO

SELECT * FROM [SQM_GENERAL].[VW_PRODUCTS];
