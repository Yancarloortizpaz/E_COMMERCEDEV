USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_SubCategories_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT subCategoryId, subCategoryName, subCategoryDescription, subCategoryStatusId
    FROM [SQM_CATALOGS].[Tbl_SubCategories] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR subCategoryName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR subCategoryStatusId = @StatusId);
END
GO
