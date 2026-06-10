USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserPaymentMethods_Create]
    @userPaymentMethodUserId INT, @userPaymentMethodPaymentMethodTypeId INT, 
    @CardNumberPlain VARCHAR(256), @ExpirationDatePlain VARCHAR(256), @CVVPlain VARCHAR(256), 
    @userPaymentMethodCardHolderName VARCHAR(100), @userPaymentMethodCreatorId INT, @userPaymentMethodStatusId BIT
AS
BEGIN
    -- Encriptamos los datos sensibles de la tarjeta
    OPEN SYMMETRIC KEY KEY_HASH DECRYPTION BY CERTIFICATE CERT_ECOMMERCE;
    
    DECLARE @CardNumberEncrypted VARBINARY(256) = SQM_SECURITY.Fn_EncryptByKey(@CardNumberPlain);
    DECLARE @ExpirationDateEncrypted VARBINARY(256) = SQM_SECURITY.Fn_EncryptByKey(@ExpirationDatePlain);
    DECLARE @CVVEncrypted VARBINARY(256) = SQM_SECURITY.Fn_EncryptByKey(@CVVPlain);

    INSERT INTO [SQM_GENERAL].[Tbl_UserPaymentMethods]
    (userPaymentMethodUserId, userPaymentMethodPaymentMethodTypeId, userPaymentMethodCardNumber, userPaymentMethodExpirationDate, userPaymentMethodCVV, userPaymentMethodCardHolderName, userPaymentMethodCreatorId, userPaymentMethodCreationDate, userPaymentMethodStatusId)
    VALUES
    (@userPaymentMethodUserId, @userPaymentMethodPaymentMethodTypeId, @CardNumberEncrypted, @ExpirationDateEncrypted, @CVVEncrypted, @userPaymentMethodCardHolderName, @userPaymentMethodCreatorId, GETDATE(), @userPaymentMethodStatusId);

    CLOSE SYMMETRIC KEY KEY_HASH;
END
GO
