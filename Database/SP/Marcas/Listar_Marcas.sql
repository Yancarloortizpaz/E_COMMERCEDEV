USE [DB_ECOMMERCE]
GO

-- 4. LISTAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Marks_List]
AS BEGIN
    SELECT markId, markName, markDescription, markCreatorId, markCreationDate, markModificatorId, markModificationDate, markStatusId
    FROM [SQM_CATALOGS].[Tbl_Marks] (NOLOCK);
END
GO
