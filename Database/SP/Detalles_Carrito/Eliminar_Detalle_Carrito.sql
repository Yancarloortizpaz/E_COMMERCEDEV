USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Físico)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_CartDetails_Delete]
    @cartDetailId INT
AS BEGIN
    DELETE FROM [SQM_GENERAL].[Tbl_CartDetails] 
    WHERE cartDetailId = @cartDetailId;
END
GO
