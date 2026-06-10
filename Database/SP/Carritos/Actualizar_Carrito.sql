USE [DB_ECOMMERCE]
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Carts_Update]
    @cartId INT, @cartUserId INT, @cartModificatorId INT, @cartStatusId BIT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_Carts] 
    SET cartUserId = @cartUserId, cartModificatorId = @cartModificatorId, cartModificationDate = GETDATE(), cartStatusId = @cartStatusId
    WHERE cartId = @cartId;
END
GO
