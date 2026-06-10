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
