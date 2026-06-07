USE [DB_ECOMMERCE]
GO

-- 1. CREAR
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PaymentOrders_Create]
    @orderUserId INT, @orderDeliveryAddress INT, @orderPaymentMethodId INT, 
    @orderSubtotal DECIMAL(18,2), @orderDiscount DECIMAL(18,2), @orderShipping DECIMAL(18,2), 
    @orderTAX DECIMAL(18,2), @orderTotal DECIMAL(18,2), @orderCurrencyId INT, 
    @orderCreatorId INT, @orderStatusId INT
AS BEGIN
    INSERT INTO [SQM_GENERAL].[Tbl_PaymentOrders] 
    (orderUserId, orderDeliveryAddress, orderPaymentMethodId, orderSubtotal, orderDiscount, orderShipping, orderTAX, orderTotal, orderCurrencyId, orderCreatorId, orderCreationDate, orderStatusId)
    VALUES 
    (@orderUserId, @orderDeliveryAddress, @orderPaymentMethodId, @orderSubtotal, @orderDiscount, @orderShipping, @orderTAX, @orderTotal, @orderCurrencyId, @orderCreatorId, GETDATE(), @orderStatusId);
END
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