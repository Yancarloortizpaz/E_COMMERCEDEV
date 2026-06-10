USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Currencies_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT currencyId, currencyName, currencyISO, currencyCode, currencyStatusId
    FROM [SQM_CATALOGS].[Tbl_Currencies] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR currencyName LIKE '%' + @SearchTerm + '%' OR currencyISO LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR currencyStatusId = @StatusId);
END
GO
