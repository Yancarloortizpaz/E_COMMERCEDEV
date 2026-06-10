USE [DB_ECOMMERCE]
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Currencies_List]
AS BEGIN
    SELECT currencyId, currencyName, currencyISO, currencyCode, currencyDescription, currencyCreatorId, currencyCreationDate, currencyModificatorId, currencyModificationDate, currencyStatusId
    FROM [SQM_CATALOGS].[Tbl_Currencies] (NOLOCK);
END
GO
