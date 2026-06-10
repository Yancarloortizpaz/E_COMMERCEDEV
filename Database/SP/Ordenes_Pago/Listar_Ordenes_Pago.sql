USE [DB_ECOMMERCE]
GO

-- 2. LISTAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PaymentOrders_List]
AS BEGIN
    SELECT 
        orderId, orderUserId, orderDeliveryAddress, orderPaymentMethodId, 
        orderSubtotal, orderDiscount, orderShipping, orderTAX, orderTotal, 
        orderCurrencyId, orderCreatorId, orderCreationDate, orderStatusId
    FROM [SQM_GENERAL].[Tbl_PaymentOrders] (NOLOCK);
END
GO
