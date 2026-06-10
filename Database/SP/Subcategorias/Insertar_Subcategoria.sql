USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_SubCategories_Create]
    @subCategoryName VARCHAR(50), @subCategoryDescription VARCHAR(100), @subCategoryCreatorId INT, @subCategoryStatusId BIT
AS BEGIN
    INSERT INTO [SQM_CATALOGS].[Tbl_SubCategories] (subCategoryName, subCategoryDescription, subCategoryCreatorId, subCategoryCreationDate, subCategoryStatusId)
    VALUES (@subCategoryName, @subCategoryDescription, @subCategoryCreatorId, GETDATE(), @subCategoryStatusId);
END
GO
