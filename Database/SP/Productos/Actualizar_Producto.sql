USE [DB_ECOMMERCE]
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_Update]
    @productId INT, @productName VARCHAR(50), @productDescription VARCHAR(200),
    @productProductIdentificatorId INT, @productMarkByProviderId INT, @productModificatorId INT, @productStatusId BIT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_Products] SET productName = @productName, productDescription = @productDescription, productProductIdentificatorId = @productProductIdentificatorId, productMarkByProviderId = @productMarkByProviderId, productModificatorId = @productModificatorId, productModificationDate = GETDATE(), productStatusId = @productStatusId
    WHERE productId = @productId;
END
GO
