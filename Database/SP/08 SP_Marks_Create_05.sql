USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Marks_Create]
    @markName VARCHAR(50), @markDescription VARCHAR(100), @markCreatorId INT, @markStatusId BIT
AS BEGIN
    INSERT INTO [SQM_CATALOGS].[Tbl_Marks] (markName, markDescription, markCreatorId, markCreationDate, markStatusId)
    VALUES (@markName, @markDescription, @markCreatorId, GETDATE(), @markStatusId);
END
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Marks_Update]
    @markId INT, @markName VARCHAR(50), @markDescription VARCHAR(100), @markModificatorId INT, @markStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Marks] SET markName = @markName, markDescription = @markDescription, markModificatorId = @markModificatorId, markModificationDate = GETDATE(), markStatusId = @markStatusId
    WHERE markId = @markId;
END
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Marks_Delete]
    @markId INT, @markModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Marks] SET markStatusId = 0, markModificatorId = @markModificatorId, markModificationDate = GETDATE() WHERE markId = @markId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Marks_List]
AS BEGIN
    SELECT markId, markName, markDescription, markCreatorId, markCreationDate, markModificatorId, markModificationDate, markStatusId
    FROM [SQM_CATALOGS].[Tbl_Marks] (NOLOCK);
END
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Marks_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT markId, markName, markDescription, markStatusId
    FROM [SQM_CATALOGS].[Tbl_Marks] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR markName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR markStatusId = @StatusId);
END
GO