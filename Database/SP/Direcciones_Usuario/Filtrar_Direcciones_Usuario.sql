USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR (Integrando Vista)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserAddress_Filter]
    @UserId INT = NULL, @StatusId BIT = NULL
AS
BEGIN
    SELECT 
        userAddressId,
        userId,
        userFullName,
        userName,
        userEmail,
        countryId,
        zipCode,
        addressDescription,
        isPrincipal,
        statusId
    FROM [SQM_GENERAL].[VW_USER_ADDRESSES] (NOLOCK)
    WHERE (@UserId IS NULL OR userId = @UserId)
      AND (@StatusId IS NULL OR statusId = @StatusId);
END
GO
