USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_Create]
    @categoryName VARCHAR(50), @categoryDescription VARCHAR(100), @categoryCreatorId INT, @categoryStatusId BIT
AS BEGIN
    INSERT INTO [SQM_CATALOGS].[Tbl_Categories] (categoryName, categoryDescription, categoryCreatorId, categoryCreationDate, categoryStatusId)
    VALUES (@categoryName, @categoryDescription, @categoryCreatorId, GETDATE(), @categoryStatusId);
END
GO
