USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR (Optimizado)
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Providers_Filter]
    @SearchTerm VARCHAR(50) = NULL, @StatusId BIT = NULL
AS BEGIN
    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

    SELECT providerId, providerName, providerDescription, providerStatusId
    FROM [SQM_CATALOGS].[Tbl_Providers] (NOLOCK)
    WHERE (
        @SearchTerm IS NULL
        OR providerId = @SearchId
        OR providerName LIKE '%' + @SearchTerm + '%'
        OR providerDescription LIKE '%' + @SearchTerm + '%'
    ) AND (@StatusId IS NULL OR providerStatusId = @StatusId)
    OPTION (RECOMPILE);
END
GO
