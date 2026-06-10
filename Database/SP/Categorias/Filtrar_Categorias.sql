USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT categoryId, categoryName, categoryDescription, categoryStatusId
    FROM [SQM_CATALOGS].[Tbl_Categories] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR categoryName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR categoryStatusId = @StatusId);
END
GO
