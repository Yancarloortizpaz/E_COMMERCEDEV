USE [DB_ECOMMERCE]
GO

-- 3. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PaymentOrders_Filter]
    @UserId INT = NULL, @StatusId INT = NULL
AS BEGIN
    SELECT 
        orderId, orderUserId, orderDeliveryAddress, orderPaymentMethodId, 
        orderSubtotal, orderDiscount, orderShipping, orderTAX, orderTotal, 
        orderCurrencyId, orderCreationDate, orderStatusId
    FROM [SQM_GENERAL].[Tbl_PaymentOrders] (NOLOCK)
    WHERE (@UserId IS NULL OR orderUserId = @UserId) 
      AND (@StatusId IS NULL OR orderStatusId = @StatusId);
END
GO
