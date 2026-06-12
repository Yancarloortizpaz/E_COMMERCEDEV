USE [DB_ECOMMERCE]
GO

-- 3. FILTRAR (Integrando Vista)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PaymentOrders_Filter]
    @UserId INT = NULL, @StatusId INT = NULL
AS BEGIN
    SELECT 
        orderId,
        userId,
        userFullName,
        userName,
        deliveryAddressId,
        deliveryAddressDescription,
        paymentMethodId,
        paymentMethodCardHolderName,
        subtotal,
        discount,
        shipping,
        tax,
        total,
        currencyId,
        currencyISO,
        creationDate,
        statusId,
        statusName
    FROM [SQM_GENERAL].[VW_PAYMENT_ORDERS] (NOLOCK)
    WHERE (@UserId IS NULL OR userId = @UserId) 
      AND (@StatusId IS NULL OR statusId = @StatusId);
END
GO
