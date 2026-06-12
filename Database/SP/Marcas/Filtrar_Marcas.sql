USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR (Optimizado)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Marks_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

    SELECT markId, markName, markDescription, markStatusId
    FROM [SQM_CATALOGS].[Tbl_Marks] (NOLOCK)
    WHERE (
        @SearchTerm IS NULL
        OR markId = @SearchId
        OR markName LIKE '%' + @SearchTerm + '%'
        OR markDescription LIKE '%' + @SearchTerm + '%'
    ) AND (@StatusId IS NULL OR markStatusId = @StatusId)
    OPTION (RECOMPILE);
END
GO
