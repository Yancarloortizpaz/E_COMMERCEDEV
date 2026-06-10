USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Físico)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Carts_Delete]
    @cartId INT
AS BEGIN
    -- Al ser un borrado físico, se elimina el registro permanentemente
    DELETE FROM [SQM_GENERAL].[Tbl_Carts] 
    WHERE cartId = @cartId;
END
GO
