USE [DB_ECOMMERCE]
GO

-- 2. EDITAR (Ajustar sumando o restando)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Stocks_Update]
    @stockId INT, 
    @stockQuantityAdjustment INT, -- Valor positivo suma (entrada), valor negativo resta (salida)
    @stockModificatorId INT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_Stocks] 
    SET stockQuantity = stockQuantity + @stockQuantityAdjustment, 
        stockModificatorId = @stockModificatorId, 
        stockModificationDate = GETDATE()
    WHERE stockId = @stockId;
END
GO
