USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_SubCategories_Delete]
    @subCategoryId INT, @subCategoryModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_SubCategories] SET subCategoryStatusId = 0, subCategoryModificatorId = @subCategoryModificatorId, subCategoryModificationDate = GETDATE() WHERE subCategoryId = @subCategoryId;
END
GO
