USE [DB_ECOMMERCE]
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
