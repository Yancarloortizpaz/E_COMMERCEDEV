USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Marks_Delete]
    @markId INT, @markModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Marks] SET markStatusId = 0, markModificatorId = @markModificatorId, markModificationDate = GETDATE() WHERE markId = @markId;
END
GO
