USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Segments_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT segmentId, segmentName, segmentDescription, segmentStatusId
    FROM [SQM_CATALOGS].[Tbl_Segments] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR segmentName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR segmentStatusId = @StatusId);
END
GO
