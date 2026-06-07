USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_Create]
    @categoryName VARCHAR(50), @categoryDescription VARCHAR(100), @categoryCreatorId INT, @categoryStatusId BIT
AS BEGIN
    INSERT INTO [SQM_CATALOGS].[Tbl_Categories] (categoryName, categoryDescription, categoryCreatorId, categoryCreationDate, categoryStatusId)
    VALUES (@categoryName, @categoryDescription, @categoryCreatorId, GETDATE(), @categoryStatusId);
END
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_Update]
    @categoryId INT, @categoryName VARCHAR(50), @categoryDescription VARCHAR(100), @categoryModificatorId INT, @categoryStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Categories] SET categoryName = @categoryName, categoryDescription = @categoryDescription, categoryModificatorId = @categoryModificatorId, categoryModificationDate = GETDATE(), categoryStatusId = @categoryStatusId
    WHERE categoryId = @categoryId;
END
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_Delete]
    @categoryId INT, @categoryModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Categories] SET categoryStatusId = 0, categoryModificatorId = @categoryModificatorId, categoryModificationDate = GETDATE() WHERE categoryId = @categoryId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_List]
AS BEGIN
    SELECT categoryId, categoryName, categoryDescription, categoryCreatorId, categoryStatusId
    FROM [SQM_CATALOGS].[Tbl_Categories] (NOLOCK);
END
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT categoryId, categoryName, categoryDescription, categoryStatusId
    FROM [SQM_CATALOGS].[Tbl_Categories] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR categoryName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR categoryStatusId = @StatusId);
END
GO