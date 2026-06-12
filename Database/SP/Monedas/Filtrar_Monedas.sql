USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR (Optimizado)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Currencies_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

    SELECT currencyId, currencyName, currencyISO, currencyCode, currencyStatusId
    FROM [SQM_CATALOGS].[Tbl_Currencies] (NOLOCK)
    WHERE (
        @SearchTerm IS NULL
        OR currencyId = @SearchId
        OR currencyName LIKE '%' + @SearchTerm + '%'
        OR currencyISO LIKE '%' + @SearchTerm + '%'
        OR currencyCode LIKE '%' + @SearchTerm + '%'
    ) AND (@StatusId IS NULL OR currencyStatusId = @StatusId)
    OPTION (RECOMPILE);
END
GO

exec [SQM_CATALOGS].[sp_Currencies_Filter]
exec [SQM_CATALOGS].[sp_Currencies_Filter]'nio'