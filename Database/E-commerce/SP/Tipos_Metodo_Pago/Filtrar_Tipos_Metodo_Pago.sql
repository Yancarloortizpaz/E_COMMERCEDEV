USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR (Optimizado)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_PaymentMethodTypes_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

    SELECT paymentMethodTypeId, paymentMethodTypeName, paymentMethodTypeDescription, paymentMethodTypeStatusId
    FROM [SQM_CATALOGS].[Tbl_PaymentMethodTypes] (NOLOCK)
    WHERE (
        @SearchTerm IS NULL
        OR paymentMethodTypeId = @SearchId
        OR paymentMethodTypeName LIKE '%' + @SearchTerm + '%'
        OR paymentMethodTypeDescription LIKE '%' + @SearchTerm + '%'
    ) AND (@StatusId IS NULL OR paymentMethodTypeStatusId = @StatusId)
    OPTION (RECOMPILE);
END
GO
 exec [SQM_CATALOGS].[sp_PaymentMethodTypes_Filter] '1'
 go

  exec [SQM_CATALOGS].[sp_PaymentMethodTypes_Filter] 'cre'
 go