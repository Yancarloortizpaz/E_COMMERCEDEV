USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_Delete]
    @categoryId INT, @categoryModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Categories] SET categoryStatusId = 0, categoryModificatorId = @categoryModificatorId, categoryModificationDate = GETDATE() WHERE categoryId = @categoryId;
END
GO
