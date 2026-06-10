USE [DB_ECOMMERCE]
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
