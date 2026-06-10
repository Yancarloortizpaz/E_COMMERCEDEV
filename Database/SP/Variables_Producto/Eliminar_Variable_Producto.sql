USE [DB_ECOMMERCE]
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductVariables_Delete]
    @productVariableId INT, @productVariableModificatorId INT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_ProductVariables] SET productVariableStatusId = 0, productVariableModificatorId = @productVariableModificatorId, productVariableModificationDate = GETDATE() WHERE productVariableId = @productVariableId;
END
GO
