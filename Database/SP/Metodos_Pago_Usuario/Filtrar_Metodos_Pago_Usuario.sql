USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR (Integrando Vista)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserPaymentMethods_Filter]
    @UserId INT = NULL, @StatusId BIT = NULL
AS
BEGIN
    OPEN SYMMETRIC KEY KEY_HASH DECRYPTION BY CERTIFICATE CERT_ECOMMERCE;

    SELECT 
        userPaymentMethodId,
        userId,
        userFullName,
        userName,
        paymentMethodTypeId,
        paymentMethodTypeName,
        cardNumberDecrypted,
        expirationDateDecrypted,
        cvvDecrypted,
        cardHolderName,
        statusId
    FROM [SQM_GENERAL].[VW_USER_PAYMENT_METHODS] (NOLOCK)
    WHERE (@UserId IS NULL OR userId = @UserId)
      AND (@StatusId IS NULL OR statusId = @StatusId);
      
    CLOSE SYMMETRIC KEY KEY_HASH;
END
GO
