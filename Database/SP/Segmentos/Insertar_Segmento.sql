USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Segments_Create]
    @segmentName VARCHAR(50), @segmentDescription VARCHAR(100), @segmentCreatorId INT, @segmentStatusId BIT
AS BEGIN
    INSERT INTO [SQM_CATALOGS].[Tbl_Segments] (segmentName, segmentDescription, segmentCreatorId, segmentCreationDate, segmentStatusId)
    VALUES (@segmentName, @segmentDescription, @segmentCreatorId, GETDATE(), @segmentStatusId);
END
GO
