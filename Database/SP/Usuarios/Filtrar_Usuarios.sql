USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR (Optimizado)
CREATE OR ALTER PROCEDURE [SQM_SECURITY].[sp_Users_Filter]
    @SearchTerm VARCHAR(100) = NULL
AS
BEGIN
    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

    SELECT userId, userFullName, userName, userEmail, userPhoneNumber, userBirthDay, userStatusId
    FROM [SQM_SECURITY].[Tbl_Users] (NOLOCK)
    WHERE (
        @SearchTerm IS NULL
        OR userId = @SearchId
        OR userFullName LIKE '%' + @SearchTerm + '%'
        OR userName LIKE '%' + @SearchTerm + '%'
        OR userEmail LIKE '%' + @SearchTerm + '%'
        OR userPhoneNumber LIKE '%' + @SearchTerm + '%'
    )
    OPTION (RECOMPILE);
END
GO
