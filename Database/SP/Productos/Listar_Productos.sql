USE [DB_ECOMMERCE]
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_List]
AS BEGIN
    SELECT productId, productName, productDescription, productProductIdentificatorId, productMarkByProviderId, productCreatorId, productCreationDate, productStatusId
    FROM [SQM_GENERAL].[Tbl_Products] (NOLOCK);
END
GO
