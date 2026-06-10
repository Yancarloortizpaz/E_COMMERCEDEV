USE [DB_ECOMMERCE]
GO

-- 2. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PaymentOrderDetails_List]
AS BEGIN
    SELECT 
        orderDetailId, orderDetailOrderId, orderDetailProductVariableId, orderDetailPrice, 
        orderDetailQuantity, orderDetailDiscount, orderDetailSubTotal, orderDetailTAX, 
        orderDetailTotal, orderDetailCurrencyId, orderDetailCreatorId, orderDetailCreationDate, 
        orderDetailStatusId
    FROM [SQM_GENERAL].[Tbl_PaymentOrderDetails] (NOLOCK);
END
GO
