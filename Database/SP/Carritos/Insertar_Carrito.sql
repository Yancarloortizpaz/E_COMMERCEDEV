USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Carts_Create]
    @cartUserId INT, @cartCreatorId INT, @cartStatusId BIT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_Carts] 
    (cartUserId, cartCreatorId, cartCreationDate, cartStatusId)
    VALUES 
    (@cartUserId, @cartCreatorId, GETDATE(), @cartStatusId);
END
GO
