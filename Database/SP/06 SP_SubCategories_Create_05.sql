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

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_SubCategories_Update]
    @subCategoryId INT, @subCategoryName VARCHAR(50), @subCategoryDescription VARCHAR(100), @subCategoryModificatorId INT, @subCategoryStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_SubCategories] SET subCategoryName = @subCategoryName, subCategoryDescription = @subCategoryDescription, subCategoryModificatorId = @subCategoryModificatorId, subCategoryModificationDate = GETDATE(), subCategoryStatusId = @subCategoryStatusId
    WHERE subCategoryId = @subCategoryId;
END
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_SubCategories_Delete]
    @subCategoryId INT, @subCategoryModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_SubCategories] SET subCategoryStatusId = 0, subCategoryModificatorId = @subCategoryModificatorId, subCategoryModificationDate = GETDATE() WHERE subCategoryId = @subCategoryId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_SubCategories_List]
AS BEGIN
    SELECT subCategoryId, subCategoryName, subCategoryDescription, subCategoryCreatorId, subCategoryStatusId
    FROM [SQM_CATALOGS].[Tbl_SubCategories] (NOLOCK);
END
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_SubCategories_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT subCategoryId, subCategoryName, subCategoryDescription, subCategoryStatusId
    FROM [SQM_CATALOGS].[Tbl_SubCategories] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR subCategoryName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR subCategoryStatusId = @StatusId);
END
GO