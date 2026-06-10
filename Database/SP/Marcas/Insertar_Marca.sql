USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Marks_Create]
    @markName VARCHAR(50), @markDescription VARCHAR(100), @markCreatorId INT, @markStatusId BIT
AS BEGIN
    INSERT INTO [SQM_CATALOGS].[Tbl_Marks] (markName, markDescription, markCreatorId, markCreationDate, markStatusId)
    VALUES (@markName, @markDescription, @markCreatorId, GETDATE(), @markStatusId);
END
GO
