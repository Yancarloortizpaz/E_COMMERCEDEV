USE [DB_ECOMMERCE]
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Marks_Update]
    @markId INT, @markName VARCHAR(50), @markDescription VARCHAR(100), @markModificatorId INT, @markStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Marks] SET markName = @markName, markDescription = @markDescription, markModificatorId = @markModificatorId, markModificationDate = GETDATE(), markStatusId = @markStatusId
    WHERE markId = @markId;
END
GO
