USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_PaymentMethodTypes_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT paymentMethodTypeId, paymentMethodTypeName, paymentMethodTypeDescription, paymentMethodTypeStatusId
    FROM [SQM_CATALOGS].[Tbl_PaymentMethodTypes] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR paymentMethodTypeName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR paymentMethodTypeStatusId = @StatusId);
END
GO
