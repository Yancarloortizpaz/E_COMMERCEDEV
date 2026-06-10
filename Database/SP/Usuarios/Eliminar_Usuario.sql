USE [DB_ECOMMERCE]
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
