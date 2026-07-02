USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_StockMovementTypes_Filter]
    @stockMovementTypeId INT = NULL,
    @stockMovementTypeName VARCHAR(50) = NULL,
    @stockMovementTypeDescription VARCHAR(100) = NULL,
    @stockMovementTypeCreatorId INT = NULL,
    @stockMovementTypeCreationDate DATETIME = NULL,
    @stockMovementTypeModificatorId INT = NULL,
    @stockMovementTypeModificationDate DATETIME = NULL,
    @stockMovementTypeStatusId BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT stockMovementTypeId, stockMovementTypeName, stockMovementTypeDescription, stockMovementTypeCreatorId, stockMovementTypeCreationDate, stockMovementTypeModificatorId, stockMovementTypeModificationDate, stockMovementTypeStatusId 
    FROM [SQM_CATALOGS].[Tbl_StockMovementTypes] (NOLOCK)
    WHERE (@stockMovementTypeId IS NULL OR stockMovementTypeId = @stockMovementTypeId)
      AND (@stockMovementTypeName IS NULL OR stockMovementTypeName LIKE '%' + TRIM(@stockMovementTypeName) + '%')
      AND (@stockMovementTypeDescription IS NULL OR stockMovementTypeDescription LIKE '%' + TRIM(@stockMovementTypeDescription) + '%')
      AND (@stockMovementTypeCreatorId IS NULL OR stockMovementTypeCreatorId = @stockMovementTypeCreatorId)
      AND (@stockMovementTypeCreationDate IS NULL OR CAST(stockMovementTypeCreationDate AS DATE) = CAST(@stockMovementTypeCreationDate AS DATE))
      AND (@stockMovementTypeModificatorId IS NULL OR stockMovementTypeModificatorId = @stockMovementTypeModificatorId)
      AND (@stockMovementTypeModificationDate IS NULL OR CAST(stockMovementTypeModificationDate AS DATE) = CAST(@stockMovementTypeModificationDate AS DATE))
      AND (@stockMovementTypeStatusId IS NULL OR stockMovementTypeStatusId = @stockMovementTypeStatusId)
    OPTION (RECOMPILE);
END;
GO