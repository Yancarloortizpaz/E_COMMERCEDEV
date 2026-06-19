USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_GetByProductVariable]
(
    @ProductVariableId INT
)
AS 
BEGIN
    SET NOCOUNT ON;

    SELECT 
        attributeProductVariableId,
        attributeProductVariableProductVariableId,
        attributeProductVariableAttributeProductId,
        attributeProductVariableValue,
        attributeProductVariableCreatorId,
        attributeProductVariableCreationDate,
        attributeProductVariableModificatorId,
        attributeProductVariableModificationDate,
        attributeProductVariableStatusId
    FROM [SQM_GENERAL].[Tbl_AttributeProductVariables] (NOLOCK)
    WHERE attributeProductVariableProductVariableId = @ProductVariableId
      AND attributeProductVariableStatusId = 1;
END
GO