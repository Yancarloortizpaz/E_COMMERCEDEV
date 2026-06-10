USE [DB_ECOMMERCE]
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Segments_Update]
    @segmentId INT, @segmentName VARCHAR(50), @segmentDescription VARCHAR(100), @segmentModificatorId INT, @segmentStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Segments] SET segmentName = @segmentName, segmentDescription = @segmentDescription, segmentModificatorId = @segmentModificatorId, segmentModificationDate = GETDATE(), segmentStatusId = @segmentStatusId
    WHERE segmentId = @segmentId;
END
GO
