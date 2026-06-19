USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR (Optimizado)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_SubCategories_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

    SELECT subCategoryId, subCategoryName, subCategoryDescription, subCategoryStatusId
    FROM [SQM_CATALOGS].[Tbl_SubCategories] (NOLOCK)
    WHERE (
        @SearchTerm IS NULL
        OR subCategoryId = @SearchId
        OR subCategoryName LIKE '%' + @SearchTerm + '%'
        OR subCategoryDescription LIKE '%' + @SearchTerm + '%'
    ) AND (@StatusId IS NULL OR subCategoryStatusId = @StatusId)
    OPTION (RECOMPILE);
END
GO

exec  [SQM_CATALOGS].[sp_SubCategories_Filter] 'ropa'

exec  [SQM_CATALOGS].[sp_SubCategories_Filter] 