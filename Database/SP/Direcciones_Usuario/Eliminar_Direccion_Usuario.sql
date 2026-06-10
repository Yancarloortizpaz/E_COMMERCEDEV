USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_UserAddress_Delete]
    @userAddressId INT, @userAddressModificatorId INT
AS
BEGIN
    UPDATE [SQM_GENERAL].[Tbl_UserAddress]
    SET userAddressStatusId = 0, userAddressModificatorId = @userAddressModificatorId, userAddressModificationDate = GETDATE()
    WHERE userAddressId = @userAddressId;
END
GO
