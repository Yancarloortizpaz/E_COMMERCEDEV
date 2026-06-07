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

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserPaymentMethods_Delete]
    @userPaymentMethodId INT, @userPaymentMethodModificatorId INT
AS
BEGIN
    UPDATE [SQM_GENERAL].[Tbl_UserPaymentMethods]
    SET userPaymentMethodStatusId = 0, userPaymentMethodModificatorId = @userPaymentMethodModificatorId, userPaymentMethodModificationDate = GETDATE()
    WHERE userPaymentMethodId = @userPaymentMethodId;
END
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