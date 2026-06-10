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
