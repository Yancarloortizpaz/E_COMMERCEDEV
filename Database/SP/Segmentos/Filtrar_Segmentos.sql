USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR (Optimizado)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Segments_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

    SELECT segmentId, segmentName, segmentDescription, segmentStatusId
    FROM [SQM_CATALOGS].[Tbl_Segments] (NOLOCK)
    WHERE (
        @SearchTerm IS NULL
        OR segmentId = @SearchId
        OR segmentName LIKE '%' + @SearchTerm + '%'
        OR segmentDescription LIKE '%' + @SearchTerm + '%'
    ) AND (@StatusId IS NULL OR segmentStatusId = @StatusId)
    OPTION (RECOMPILE);
END
GO

exec  [SQM_CATALOGS].[sp_Segments_Filter]