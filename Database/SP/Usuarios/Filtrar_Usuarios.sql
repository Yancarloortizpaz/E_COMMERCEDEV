USE [DB_ECOMMERCE]
GO

-- 5. FILTRAR
CREATE OR ALTER PROCEDURE [SQM_SECURITY].[sp_Users_Filter]
    @SearchTerm VARCHAR(100) = NULL
AS
BEGIN
    SELECT userId, userFullName, userName, userEmail, userPhoneNumber, userBirthDay, userStatusId
    FROM [SQM_SECURITY].[Tbl_Users] (NOLOCK)
    WHERE (@SearchTerm IS NULL OR userName LIKE '%' + @SearchTerm + '%' OR userEmail LIKE '%' + @SearchTerm + '%');
END
GO
