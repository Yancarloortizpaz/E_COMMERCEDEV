USE [DB_ECOMMERCE]
GO

-- 3. FILTRAR (Integrando Vista)
CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PaymentOrderDetails_Filter]
    @OrderId INT = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT 
        orderDetailId,
        orderId,
        productVariableId,
        productName,
        productDescription,
        categoryName,
        subCategoryName,
        segmentName,
        markName,
        providerName,
        variableValue,
        price,
        quantity,
        discount,
        subtotal,
        tax,
        total,
        currencyId,
        currencyISO,
        statusId
    FROM [SQM_GENERAL].[VW_PAYMENT_ORDER_DETAILS] (NOLOCK)
    WHERE (@OrderId IS NULL OR orderId = @OrderId) 
      AND (@StatusId IS NULL OR statusId = @StatusId);
END
GO
