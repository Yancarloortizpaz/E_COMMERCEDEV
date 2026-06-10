USE [DB_ECOMMERCE]
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_SubCategories_Update]
    @subCategoryId INT, @subCategoryName VARCHAR(50), @subCategoryDescription VARCHAR(100), @subCategoryModificatorId INT, @subCategoryStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_SubCategories] SET subCategoryName = @subCategoryName, subCategoryDescription = @subCategoryDescription, subCategoryModificatorId = @subCategoryModificatorId, subCategoryModificationDate = GETDATE(), subCategoryStatusId = @subCategoryStatusId
    WHERE subCategoryId = @subCategoryId;
END
GO
