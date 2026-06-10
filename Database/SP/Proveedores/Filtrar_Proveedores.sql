USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Providers_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    SELECT providerId, providerName, providerDescription, providerStatusId
    FROM [SQM_CATALOGS].[Tbl_Providers] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR providerName LIKE '%' + @SearchTerm + '%') AND (@StatusId IS NULL OR providerStatusId = @StatusId);
END
GO
