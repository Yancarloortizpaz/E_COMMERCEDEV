USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Carts_Filter]
    @SearchTerm VARCHAR(50) = NULL, 
    @StatusId BIT = NULL
AS 
BEGIN
    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

    SELECT 
        cartId, 
        cartUserId, 
        cartCreatorId, 
        cartCreationDate, 
        cartModificatorId, 
        cartModificationDate, 
        cartStatusId
    FROM [SQM_GENERAL].[Tbl_Carts] (NOLOCK)
    WHERE (
        @SearchTerm IS NULL
        OR cartId = @SearchId
        OR cartUserId = @SearchId
    ) AND (@StatusId IS NULL OR cartStatusId = @StatusId)
    OPTION (RECOMPILE);
END
GO


exec  [SQM_GENERAL].[sp_Carts_Filter]