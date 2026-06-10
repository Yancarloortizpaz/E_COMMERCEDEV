USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Providers_Delete]
    @providerId INT, @providerModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Providers] SET providerStatusId = 0, providerModificatorId = @providerModificatorId, providerModificationDate = GETDATE() WHERE providerId = @providerId;
END
GO
