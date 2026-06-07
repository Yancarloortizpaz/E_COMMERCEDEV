USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_SECURITY].[sp_Users_Create]
    @userFullName VARCHAR(100), @userName VARCHAR(50), @userPasswordPlain VARCHAR(256), 
    @userEmail VARCHAR(80), @userPhoneNumber VARCHAR(20), @userCountryId INT, 
    @userGenderId INT, @userBirthDay DATE, @userCreatorId INT, @userStatusId INT
AS
BEGIN
    OPEN SYMMETRIC KEY KEY_HASH DECRYPTION BY CERTIFICATE CERT_ECOMMERCE;
    DECLARE @UserPasswordEncrypted VARBINARY(256) = SQM_SECURITY.Fn_EncryptByKey(@userPasswordPlain);

    INSERT INTO [SQM_SECURITY].[Tbl_Users] 
    (userFullName, userName, userPassword, userEmail, userPhoneNumber, userCountryId, userGenderId, userBirthDay, userCreatorId, userCreationDate, userStatusId)
    VALUES 
    (@userFullName, @userName, @UserPasswordEncrypted, @userEmail, @userPhoneNumber, @userCountryId, @userGenderId, @userBirthDay, @userCreatorId, GETDATE(), @userStatusId);

    CLOSE SYMMETRIC KEY KEY_HASH;
END
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_SECURITY].[sp_Users_Update]
    @userId INT, @userFullName VARCHAR(100), @userEmail VARCHAR(80), 
    @userPhoneNumber VARCHAR(20), @userModificatorId INT, @userStatusId INT
AS
BEGIN
    UPDATE [SQM_SECURITY].[Tbl_Users]
    SET userFullName = @userFullName, userEmail = @userEmail, userPhoneNumber = @userPhoneNumber,
        userModificatorId = @userModificatorId, userModificationDate = GETDATE(), userStatusId = @userStatusId
    WHERE userId = @userId;
END
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_SECURITY].[sp_Users_Delete]
    @userId INT, @userModificatorId INT
AS
BEGIN
    UPDATE [SQM_SECURITY].[Tbl_Users]
    SET userStatusId = 0, userModificatorId = @userModificatorId, userModificationDate = GETDATE()
    WHERE userId = @userId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_SECURITY].[sp_Users_List]
AS
BEGIN
    OPEN SYMMETRIC KEY KEY_HASH DECRYPTION BY CERTIFICATE CERT_ECOMMERCE;
    
    SELECT 
        userId, userFullName, userName, 
        SQM_SECURITY.Fn_DecryptByKey(userPassword) AS [userPasswordDecrypted], 
        userEmail, userPhoneNumber, userCountryId, userGenderId, userBirthDay, userStatusId
    FROM [SQM_SECURITY].[Tbl_Users] (NOLOCK);
    
    CLOSE SYMMETRIC KEY KEY_HASH;
END
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_SECURITY].[sp_Users_Filter]
    @SearchTerm VARCHAR(100) = NULL
AS
BEGIN
    SELECT userId, userFullName, userName, userEmail, userPhoneNumber, userBirthDay, userStatusId
    FROM [SQM_SECURITY].[Tbl_Users] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR userName LIKE '%' + @SearchTerm + '%' OR userEmail LIKE '%' + @SearchTerm + '%');
END
GO