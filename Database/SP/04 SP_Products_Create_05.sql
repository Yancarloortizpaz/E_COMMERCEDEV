USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_Create]
    @productName VARCHAR(50), @productDescription VARCHAR(200), @productProductIdentificatorId INT,
    @productMarkByProviderId INT, @productCreatorId INT, @productStatusId BIT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_Products] (productName, productDescription, productProductIdentificatorId, productMarkByProviderId, productCreatorId, productCreationDate, productStatusId)
    VALUES (@productName, @productDescription, @productProductIdentificatorId, @productMarkByProviderId, @productCreatorId, GETDATE(), @productStatusId);
END
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_Update]
    @productId INT, @productName VARCHAR(50), @productDescription VARCHAR(200),
    @productProductIdentificatorId INT, @productMarkByProviderId INT, @productModificatorId INT, @productStatusId BIT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_Products] SET productName = @productName, productDescription = @productDescription, productProductIdentificatorId = @productProductIdentificatorId, productMarkByProviderId = @productMarkByProviderId, productModificatorId = @productModificatorId, productModificationDate = GETDATE(), productStatusId = @productStatusId
    WHERE productId = @productId;
END
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_Delete]
    @productId INT, @productModificatorId INT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_Products] SET productStatusId = 0, productModificatorId = @productModificatorId, productModificationDate = GETDATE() WHERE productId = @productId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_List]
AS BEGIN
    SELECT productId, productName, productDescription, productProductIdentificatorId, productMarkByProviderId, productCreatorId, productCreationDate, productStatusId
    FROM [SQM_GENERAL].[Tbl_Products] (NOLOCK);
END
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT productId, productName, productDescription, productStatusId
    FROM [SQM_GENERAL].[Tbl_Products] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR productName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR productStatusId = @StatusId);
END
GO