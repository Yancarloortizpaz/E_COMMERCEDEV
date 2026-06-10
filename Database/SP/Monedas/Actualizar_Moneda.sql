USE [DB_ECOMMERCE]
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Currencies_Update]
    @currencyId INT, @currencyName VARCHAR(50), @currencyISO CHAR(5), @currencyCode INT, @currencyDescription VARCHAR(100), @currencyModificatorId INT, @currencyStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Currencies] SET currencyName = @currencyName, currencyISO = @currencyISO, currencyCode = @currencyCode, currencyDescription = @currencyDescription, currencyModificatorId = @currencyModificatorId, currencyModificationDate = GETDATE(), currencyStatusId = @currencyStatusId
    WHERE currencyId = @currencyId;
END
GO
