USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Providers_Create]
    @providerName VARCHAR(50), @providerDescription VARCHAR(100), @providerCreatorId INT, @providerStatusId BIT
AS BEGIN
    INSERT INTO [SQM_CATALOGS].[Tbl_Providers] (providerName, providerDescription, providerCreatorId, providerCreationDate, providerStatusId)
    VALUES (@providerName, @providerDescription, @providerCreatorId, GETDATE(), @providerStatusId);
END
GO
