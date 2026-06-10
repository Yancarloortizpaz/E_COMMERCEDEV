USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Segments_Delete]
    @segmentId INT, @segmentModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Segments] SET segmentStatusId = 0, segmentModificatorId = @segmentModificatorId, segmentModificationDate = GETDATE() WHERE segmentId = @segmentId;
END
GO
