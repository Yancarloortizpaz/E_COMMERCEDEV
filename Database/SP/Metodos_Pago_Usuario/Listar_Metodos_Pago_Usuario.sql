USE [DB_ECOMMERCE]
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserPaymentMethods_List]
AS
BEGIN
    OPEN SYMMETRIC KEY KEY_HASH DECRYPTION BY CERTIFICATE CERT_ECOMMERCE;

    SELECT 
        userPaymentMethodId, userPaymentMethodUserId, userPaymentMethodPaymentMethodTypeId, 
        SQM_SECURITY.Fn_DecryptByKey(userPaymentMethodCardNumber) AS [CardNumberDecrypted],
        SQM_SECURITY.Fn_DecryptByKey(userPaymentMethodExpirationDate) AS [ExpirationDateDecrypted],
        SQM_SECURITY.Fn_DecryptByKey(userPaymentMethodCVV) AS [CVVDecrypted],
        userPaymentMethodCardHolderName, userPaymentMethodCreatorId, userPaymentMethodCreationDate, userPaymentMethodStatusId
    FROM [SQM_GENERAL].[Tbl_UserPaymentMethods] (NOLOCK);

    CLOSE SYMMETRIC KEY KEY_HASH;
END
GO
