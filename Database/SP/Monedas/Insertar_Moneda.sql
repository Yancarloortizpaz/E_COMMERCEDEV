USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Currencies_Create]
    @currencyName VARCHAR(50), @currencyISO CHAR(5), @currencyCode INT, @currencyDescription VARCHAR(100), @currencyCreatorId INT, @currencyStatusId BIT
AS BEGIN
    INSERT INTO [SQM_CATALOGS].[Tbl_Currencies] (currencyName, currencyISO, currencyCode, currencyDescription, currencyCreatorId, currencyCreationDate, currencyStatusId)
    VALUES (@currencyName, @currencyISO, @currencyCode, @currencyDescription, @currencyCreatorId, GETDATE(), @currencyStatusId);
END
GO
