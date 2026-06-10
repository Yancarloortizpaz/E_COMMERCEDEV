USE [DB_ECOMMERCE]
GO

-- 2. EDITAR
CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Categories_Update]
    @categoryId INT, @categoryName VARCHAR(50), @categoryDescription VARCHAR(100), @categoryModificatorId INT, @categoryStatusId BIT
AS BEGIN
    UPDATE [SQM_CATALOGS].[Tbl_Categories] SET categoryName = @categoryName, categoryDescription = @categoryDescription, categoryModificatorId = @categoryModificatorId, categoryModificationDate = GETDATE(), categoryStatusId = @categoryStatusId
    WHERE categoryId = @categoryId;
END
GO
