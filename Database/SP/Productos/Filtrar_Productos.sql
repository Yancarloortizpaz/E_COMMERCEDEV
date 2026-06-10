USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT productId, productName, productDescription, productStatusId
    FROM [SQM_GENERAL].[Tbl_Products] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR productName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR productStatusId = @StatusId);
END
GO
