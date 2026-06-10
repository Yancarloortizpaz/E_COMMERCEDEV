USE [DB_ECOMMERCE]
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
