USE [DB_ECOMMERCE]
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_SubCategories_List]
AS BEGIN
    SELECT subCategoryId, subCategoryName, subCategoryDescription, subCategoryCreatorId, subCategoryStatusId
    FROM [SQM_CATALOGS].[Tbl_SubCategories] (NOLOCK);
END
GO
