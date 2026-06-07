USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductVariables_Create]
    @productVariableProductId INT, @productVariableValue VARCHAR(50), @productVariablePrice DECIMAL(18,2), @productVariableCurrencyId INT, @productVariableCreatorId INT, @productVariableStatusId BIT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_ProductVariables] (productVariableProductId, productVariableValue, productVariablePrice, productVariableCurrencyId, productVariableCreatorId, productVariableCreationDate, productVariableStatusId)
    VALUES (@productVariableProductId, @productVariableValue, @productVariablePrice, @productVariableCurrencyId, @productVariableCreatorId, GETDATE(), @productVariableStatusId);
END
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductVariables_Update]
    @productVariableId INT, @productVariableProductId INT, @productVariableValue VARCHAR(50), @productVariablePrice DECIMAL(18,2), @productVariableCurrencyId INT, @productVariableModificatorId INT, @productVariableStatusId BIT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_ProductVariables] SET productVariableProductId = @productVariableProductId, productVariableValue = @productVariableValue, productVariablePrice = @productVariablePrice, productVariableCurrencyId = @productVariableCurrencyId, productVariableModificatorId = @productVariableModificatorId, productVariableModificationDate = GETDATE(), productVariableStatusId = @productVariableStatusId
    WHERE productVariableId = @productVariableId;
END
GO

-- 3. ELIMINAR (Borrado Lógico)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductVariables_Delete]
    @productVariableId INT, @productVariableModificatorId INT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_ProductVariables] SET productVariableStatusId = 0, productVariableModificatorId = @productVariableModificatorId, productVariableModificationDate = GETDATE() WHERE productVariableId = @productVariableId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductVariables_List]
AS BEGIN
    SELECT productVariableId, productVariableProductId, productVariableValue, productVariablePrice, productVariableCurrencyId, productVariableCreatorId, productVariableCreationDate, productVariableModificatorId, productVariableModificationDate, productVariableStatusId
    FROM [SQM_GENERAL].[Tbl_ProductVariables] (NOLOCK);
END
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductVariables_Filter]
    @ProductId INT = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT productVariableId, productVariableProductId, productVariableValue, productVariablePrice, productVariableCurrencyId, productVariableStatusId
    FROM [SQM_GENERAL].[Tbl_ProductVariables] (NOLOCK)
    WHERE (@ProductId IS NULL OR productVariableProductId = @ProductId) AND (@StatusId IS NULL OR productVariableStatusId = @StatusId);
END
GO