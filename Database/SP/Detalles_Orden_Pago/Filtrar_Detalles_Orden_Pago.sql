USE [DB_ECOMMERCE]
GO

-- 3. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PaymentOrderDetails_Filter]
    @OrderId INT = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT 
        orderDetailId, orderDetailOrderId, orderDetailProductVariableId, orderDetailPrice, 
        orderDetailQuantity, orderDetailDiscount, orderDetailSubTotal, orderDetailTAX, 
        orderDetailTotal, orderDetailCurrencyId, orderDetailCreationDate, orderDetailStatusId
    FROM [SQM_GENERAL].[Tbl_PaymentOrderDetails] (NOLOCK)
    WHERE (@OrderId IS NULL OR orderDetailOrderId = @OrderId) 
      AND (@StatusId IS NULL OR orderDetailStatusId = @StatusId);
END
GO
