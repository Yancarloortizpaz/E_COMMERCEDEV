USE [DB_ECOMMERCE]
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Segments_List]
AS BEGIN
    SELECT segmentId, segmentName, segmentDescription, segmentCreatorId, segmentStatusId
    FROM [SQM_CATALOGS].[Tbl_Segments] (NOLOCK);
END
GO
