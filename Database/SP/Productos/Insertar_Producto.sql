USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_Create]
    @productName VARCHAR(50), @productDescription VARCHAR(200), @productProductIdentificatorId INT,
    @productMarkByProviderId INT, @productCreatorId INT, @productStatusId BIT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_Products] (productName, productDescription, productProductIdentificatorId, productMarkByProviderId, productCreatorId, productCreationDate, productStatusId)
    VALUES (@productName, @productDescription, @productProductIdentificatorId, @productMarkByProviderId, @productCreatorId, GETDATE(), @productStatusId);
END
GO
