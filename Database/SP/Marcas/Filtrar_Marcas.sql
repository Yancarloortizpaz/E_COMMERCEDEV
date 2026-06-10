USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Marks_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT markId, markName, markDescription, markStatusId
    FROM [SQM_CATALOGS].[Tbl_Marks] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR markName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR markStatusId = @StatusId);
END
GO
