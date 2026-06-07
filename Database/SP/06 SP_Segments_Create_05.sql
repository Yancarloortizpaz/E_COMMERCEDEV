USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Segments_Create]
    @segmentName VARCHAR(50), @segmentDescription VARCHAR(100), @segmentCreatorId INT, @segmentStatusId BIT
AS BEGIN
    INSERT INTO [SQM_CATALOGS].[Tbl_Segments] (segmentName, segmentDescription, segmentCreatorId, segmentCreationDate, segmentStatusId)
    VALUES (@segmentName, @segmentDescription, @segmentCreatorId, GETDATE(), @segmentStatusId);
END
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Segments_Update]
    @segmentId INT, @segmentName VARCHAR(50), @segmentDescription VARCHAR(100), @segmentModificatorId INT, @segmentStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Segments] SET segmentName = @segmentName, segmentDescription = @segmentDescription, segmentModificatorId = @segmentModificatorId, segmentModificationDate = GETDATE(), segmentStatusId = @segmentStatusId
    WHERE segmentId = @segmentId;
END
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Segments_Delete]
    @segmentId INT, @segmentModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Segments] SET segmentStatusId = 0, segmentModificatorId = @segmentModificatorId, segmentModificationDate = GETDATE() WHERE segmentId = @segmentId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Segments_List]
AS BEGIN
    SELECT segmentId, segmentName, segmentDescription, segmentCreatorId, segmentStatusId
    FROM [SQM_CATALOGS].[Tbl_Segments] (NOLOCK);
END
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Segments_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT segmentId, segmentName, segmentDescription, segmentStatusId
    FROM [SQM_CATALOGS].[Tbl_Segments] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR segmentName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR segmentStatusId = @StatusId);
END
GO