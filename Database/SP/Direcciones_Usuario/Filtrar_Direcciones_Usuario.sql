USE [DB_ECOMMERCE]
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
