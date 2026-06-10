USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserPaymentMethods_Filter]
    @UserId INT = NULL, @StatusId BIT = NULL
AS
BEGIN
    OPEN SYMMETRIC KEY KEY_HASH DECRYPTION BY CERTIFICATE CERT_ECOMMERCE;

    SELECT 
        userPaymentMethodId, userPaymentMethodUserId, userPaymentMethodPaymentMethodTypeId, 
        SQM_SECURITY.Fn_DecryptByKey(userPaymentMethodCardNumber) AS [CardNumberDecrypted],
        userPaymentMethodCardHolderName, userPaymentMethodStatusId
    FROM [SQM_GENERAL].[Tbl_UserPaymentMethods] (NOLOCK)
    WHERE (@UserId IS NULL OR userPaymentMethodUserId = @UserId)
      AND (@StatusId IS NULL OR userPaymentMethodStatusId = @StatusId);
      
    CLOSE SYMMETRIC KEY KEY_HASH;
END
GO
