USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_CartDetails_Create]
    @cartDetailCartId INT, @cartDetailProductVariableId INT, @cartDetailPrice DECIMAL(18,2), 
    @cartDetailQuantity INT, @cartDetailDiscount DECIMAL(18,2), @cartDetailSubTotal DECIMAL(18,2), 
    @cartDetailTAX DECIMAL(18,2), @cartDetailTotal DECIMAL(18,2), @cartDetailCurrencyId INT, 
    @cartDetailCreatorId INT, @cartDetailStatusId BIT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_CartDetails] 
    (cartDetailCartId, cartDetailProductVariableId, cartDetailPrice, cartDetailQuantity, cartDetailDiscount, cartDetailSubTotal, cartDetailTAX, cartDetailTotal, cartDetailCurrencyId, cartDetailCreatorId, cartDetailCreationDate, cartDetailStatusId)
    VALUES 
    (@cartDetailCartId, @cartDetailProductVariableId, @cartDetailPrice, @cartDetailQuantity, @cartDetailDiscount, @cartDetailSubTotal, @cartDetailTAX, @cartDetailTotal, @cartDetailCurrencyId, @cartDetailCreatorId, GETDATE(), @cartDetailStatusId);
END
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_CartDetails_Update]
    @cartDetailId INT, @cartDetailProductVariableId INT, @cartDetailPrice DECIMAL(18,2), 
    @cartDetailQuantity INT, @cartDetailDiscount DECIMAL(18,2), @cartDetailSubTotal DECIMAL(18,2), 
    @cartDetailTAX DECIMAL(18,2), @cartDetailTotal DECIMAL(18,2), @cartDetailCurrencyId INT, 
    @cartDetailModificatorId INT, @cartDetailStatusId BIT
AS BEGIN
    UPDATE [SQM_GENERAL].[Tbl_CartDetails] 
    SET cartDetailProductVariableId = @cartDetailProductVariableId, cartDetailPrice = @cartDetailPrice, 
        cartDetailQuantity = @cartDetailQuantity, cartDetailDiscount = @cartDetailDiscount, 
        cartDetailSubTotal = @cartDetailSubTotal, cartDetailTAX = @cartDetailTAX, 
        cartDetailTotal = @cartDetailTotal, cartDetailCurrencyId = @cartDetailCurrencyId, 
        cartDetailModificatorId = @cartDetailModificatorId, cartDetailModificationDate = GETDATE(), 
        cartDetailStatusId = @cartDetailStatusId
    WHERE cartDetailId = @cartDetailId;
END
GO

-- 3. ELIMINAR (Borrado Físico)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_CartDetails_Delete]
    @cartDetailId INT
AS BEGIN
    DELETE FROM [SQM_GENERAL].[Tbl_CartDetails] 
    WHERE cartDetailId = @cartDetailId;
END
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_CartDetails_List]
AS BEGIN
    SELECT 
        cartDetailId, cartDetailCartId, cartDetailProductVariableId, cartDetailPrice, 
        cartDetailQuantity, cartDetailDiscount, cartDetailSubTotal, cartDetailTAX, 
        cartDetailTotal, cartDetailCurrencyId, cartDetailCreatorId, cartDetailCreationDate, 
        cartDetailModificatorId, cartDetailModificationDate, cartDetailStatusId
    FROM [SQM_GENERAL].[Tbl_CartDetails] (NOLOCK);
END
GO