USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_Delete]
    @productId INT, @productModificatorId INT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_Products] SET productStatusId = 0, productModificatorId = @productModificatorId, productModificationDate = GETDATE() WHERE productId = @productId;
END
GO
