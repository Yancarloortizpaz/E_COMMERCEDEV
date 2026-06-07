USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Providers_Create]
    @providerName VARCHAR(50), @providerDescription VARCHAR(100), @providerCreatorId INT, @providerStatusId BIT
AS BEGIN
    INSERT INTO [SQM_CATALOGS].[Tbl_Providers] (providerName, providerDescription, providerCreatorId, providerCreationDate, providerStatusId)
    VALUES (@providerName, @providerDescription, @providerCreatorId, GETDATE(), @providerStatusId);
END
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Providers_Update]
    @providerId INT, @providerName VARCHAR(50), @providerDescription VARCHAR(100), @providerModificatorId INT, @providerStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Providers] SET providerName = @providerName, providerDescription = @providerDescription, providerModificatorId = @providerModificatorId, providerModificationDate = GETDATE(), providerStatusId = @providerStatusId
    WHERE providerId = @providerId;
END
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Providers_Delete]
    @providerId INT, @providerModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Providers] SET providerStatusId = 0, providerModificatorId = @providerModificatorId, providerModificationDate = GETDATE() WHERE providerId = @providerId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Providers_List]
AS BEGIN
    SELECT providerId, providerName, providerDescription, providerCreatorId, providerStatusId
    FROM [SQM_CATALOGS].[Tbl_Providers] (NOLOCK);
END
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Providers_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT providerId, providerName, providerDescription, providerStatusId
    FROM [SQM_CATALOGS].[Tbl_Providers] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR providerName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR providerStatusId = @StatusId);
END
GO