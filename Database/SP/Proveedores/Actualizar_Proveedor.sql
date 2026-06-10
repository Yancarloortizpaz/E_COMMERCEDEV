USE [DB_ECOMMERCE]
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Providers_Update]
    @providerId INT, @providerName VARCHAR(50), @providerDescription VARCHAR(100), @providerModificatorId INT, @providerStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Providers] SET providerName = @providerName, providerDescription = @providerDescription, providerModificatorId = @providerModificatorId, providerModificationDate = GETDATE(), providerStatusId = @providerStatusId
    WHERE providerId = @providerId;
END
GO
