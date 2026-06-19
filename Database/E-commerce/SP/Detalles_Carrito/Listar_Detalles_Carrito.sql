USE [DB_ECOMMERCE]
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
