USE [DB_ECOMMERCE]
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserPaymentMethods_Update]
    @userPaymentMethodId INT, @userPaymentMethodPaymentMethodTypeId INT, 
    @CardNumberPlain VARCHAR(256), @ExpirationDatePlain VARCHAR(256), @CVVPlain VARCHAR(256), 
    @userPaymentMethodCardHolderName VARCHAR(100), @userPaymentMethodModificatorId INT, @userPaymentMethodStatusId BIT
AS
BEGIN
    OPEN SYMMETRIC KEY KEY_HASH DECRYPTION BY CERTIFICATE CERT_ECOMMERCE;
    
    DECLARE @CardNumberEncrypted VARBINARY(256) = SQM_SECURITY.Fn_EncryptByKey(@CardNumberPlain);
    DECLARE @ExpirationDateEncrypted VARBINARY(256) = SQM_SECURITY.Fn_EncryptByKey(@ExpirationDatePlain);
    DECLARE @CVVEncrypted VARBINARY(256) = SQM_SECURITY.Fn_EncryptByKey(@CVVPlain);

    UPDATE [SQM_GENERAL].[Tbl_UserPaymentMethods]
    SET userPaymentMethodPaymentMethodTypeId = @userPaymentMethodPaymentMethodTypeId,
        userPaymentMethodCardNumber = @CardNumberEncrypted,
        userPaymentMethodExpirationDate = @ExpirationDateEncrypted,
        userPaymentMethodCVV = @CVVEncrypted,
        userPaymentMethodCardHolderName = @userPaymentMethodCardHolderName,
        userPaymentMethodModificatorId = @userPaymentMethodModificatorId,
        userPaymentMethodModificationDate = GETDATE(),
        userPaymentMethodStatusId = @userPaymentMethodStatusId
    WHERE userPaymentMethodId = @userPaymentMethodId;

    CLOSE SYMMETRIC KEY KEY_HASH;
END
GO
