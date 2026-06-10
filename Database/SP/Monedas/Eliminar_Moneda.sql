USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Currencies_Delete]
    @currencyId INT, @currencyModificatorId INT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Currencies] SET currencyStatusId = 0, currencyModificatorId = @currencyModificatorId, currencyModificationDate = GETDATE() WHERE currencyId = @currencyId;
END
GO
