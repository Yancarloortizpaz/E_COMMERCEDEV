USE [DB_ECOMMERCE]
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
