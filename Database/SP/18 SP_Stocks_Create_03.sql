USE [DB_ECOMMERCE]
GO

-- 1. CREAR (Inicializar producto en bodega)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Stocks_Create]
    @stockProductVariableId INT, 
    @stockQuantity INT, 
    @stockFactoryDate DATE, 
    @stockExpirationDate DATE, 
    @stockCreatorId INT, 
    @stockStatusId BIT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_Stocks] 
    (stockProductVariableId, stockQuantity, stockFactoryDate, stockExpirationDate, stockCreatorId, stockCreationDate, stockStatusId)
    VALUES 
    (@stockProductVariableId, @stockQuantity, @stockFactoryDate, @stockExpirationDate, @stockCreatorId, GETDATE(), @stockStatusId);
END
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

-- 3. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Stocks_List]
AS BEGIN
    SELECT 
        stockId, stockProductVariableId, stockQuantity, stockFactoryDate, 
        stockExpirationDate, stockCreatorId, stockCreationDate, 
        stockModificatorId, stockModificationDate, stockStatusId
    FROM [SQM_GENERAL].[Tbl_Stocks] (NOLOCK);
END
GO