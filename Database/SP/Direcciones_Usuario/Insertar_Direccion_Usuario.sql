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
