USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Currencies_Create]
    @currencyName VARCHAR(50), @currencyISO CHAR(5), @currencyCode INT, @currencyDescription VARCHAR(100), @currencyCreatorId INT, @currencyStatusId BIT
AS BEGIN
    INSERT INTO [SQM_CATALOGS].[Tbl_Currencies] (currencyName, currencyISO, currencyCode, currencyDescription, currencyCreatorId, currencyCreationDate, currencyStatusId)
    VALUES (@currencyName, @currencyISO, @currencyCode, @currencyDescription, @currencyCreatorId, GETDATE(), @currencyStatusId);
END
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Currencies_Update]
    @currencyId INT, @currencyName VARCHAR(50), @currencyISO CHAR(5), @currencyCode INT, @currencyDescription VARCHAR(100), @currencyModificatorId INT, @currencyStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Currencies] SET currencyName = @currencyName, currencyISO = @currencyISO, currencyCode = @currencyCode, currencyDescription = @currencyDescription, currencyModificatorId = @currencyModificatorId, currencyModificationDate = GETDATE(), currencyStatusId = @currencyStatusId
    WHERE currencyId = @currencyId;
END
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Currencies_Delete]
    @currencyId INT, @currencyModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Currencies] SET currencyStatusId = 0, currencyModificatorId = @currencyModificatorId, currencyModificationDate = GETDATE() WHERE currencyId = @currencyId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Currencies_List]
AS BEGIN
    SELECT currencyId, currencyName, currencyISO, currencyCode, currencyDescription, currencyCreatorId, currencyCreationDate, currencyModificatorId, currencyModificationDate, currencyStatusId
    FROM [SQM_CATALOGS].[Tbl_Currencies] (NOLOCK);
END
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Currencies_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT currencyId, currencyName, currencyISO, currencyCode, currencyStatusId
    FROM [SQM_CATALOGS].[Tbl_Currencies] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR currencyName LIKE '%' + @SearchTerm + '%' OR currencyISO LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR currencyStatusId = @StatusId);
END
GO