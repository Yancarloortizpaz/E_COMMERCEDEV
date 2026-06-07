USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserAddress_Create]
    @userAddressUserId INT, @userAddressCountryId INT, @userAddressZIPCode INT, 
    @userAddressDescription NVARCHAR(500), @userAddressIsPrincipal BIT, @userAddressCreatorId INT, @userAddressStatusId BIT
AS
BEGIN
    IF @userAddressIsPrincipal = 1
    BEGIN
        UPDATE [SQM_GENERAL].[Tbl_UserAddress] SET userAddressIsPrincipal = 0 WHERE userAddressUserId = @userAddressUserId;
    END

    INSERT INTO [SQM_GENERAL].[Tbl_UserAddress]
    (userAddressUserId, userAddressCountryId, userAddressZIPCode, userAddressDescription, userAddressIsPrincipal, userAddressCreatorId, userAddressCreationDate, userAddressStatusId)
    VALUES
    (@userAddressUserId, @userAddressCountryId, @userAddressZIPCode, @userAddressDescription, @userAddressIsPrincipal, @userAddressCreatorId, GETDATE(), @userAddressStatusId);
END
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserAddress_Update]
    @userAddressId INT, @userAddressCountryId INT, @userAddressZIPCode INT, 
    @userAddressDescription NVARCHAR(500), @userAddressIsPrincipal BIT, @userAddressModificatorId INT, @userAddressStatusId BIT
AS
BEGIN
    IF @userAddressIsPrincipal = 1
    BEGIN
        DECLARE @UserId INT = (SELECT userAddressUserId FROM [SQM_GENERAL].[Tbl_UserAddress] WHERE userAddressId = @userAddressId);
        UPDATE [SQM_GENERAL].[Tbl_UserAddress] SET userAddressIsPrincipal = 0 WHERE userAddressUserId = @UserId;
    END

    UPDATE [SQM_GENERAL].[Tbl_UserAddress]
    SET userAddressCountryId = @userAddressCountryId, userAddressZIPCode = @userAddressZIPCode,
        userAddressDescription = @userAddressDescription, userAddressIsPrincipal = @userAddressIsPrincipal,
        userAddressModificatorId = @userAddressModificatorId, userAddressModificationDate = GETDATE(), userAddressStatusId = @userAddressStatusId
    WHERE userAddressId = @userAddressId;
END
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserAddress_Delete]
    @userAddressId INT, @userAddressModificatorId INT
AS
BEGIN
    UPDATE [SQM_GENERAL].[Tbl_UserAddress]
    SET userAddressStatusId = 0, userAddressModificatorId = @userAddressModificatorId, userAddressModificationDate = GETDATE()
    WHERE userAddressId = @userAddressId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserAddress_List]
AS
BEGIN
    SELECT userAddressId, userAddressUserId, userAddressCountryId, userAddressZIPCode, userAddressDescription, 
           userAddressIsPrincipal, userAddressCreatorId, userAddressCreationDate, userAddressModificatorId, userAddressModificationDate, userAddressStatusId
    FROM [SQM_GENERAL].[Tbl_UserAddress] (NOLOCK);
END
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserAddress_Filter]
    @UserId INT = NULL, @StatusId BIT = NULL
AS
BEGIN
    SELECT userAddressId, userAddressUserId, userAddressCountryId, userAddressZIPCode, userAddressDescription, userAddressIsPrincipal, userAddressStatusId
    FROM [SQM_GENERAL].[Tbl_UserAddress] (NOLOCK)
    WHERE (@UserId IS NULL OR userAddressUserId = @UserId)
      AND (@StatusId IS NULL OR userAddressStatusId = @StatusId);
END
GO