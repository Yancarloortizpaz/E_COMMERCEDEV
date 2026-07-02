CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_List]
AS
BEGIN
    SELECT
        attributeProductVariableId,
        attributeProductVariableProductVariableId,
        attributeProductVariableAttributeProductId,
        attributeProductVariableValue,
        attributeProductVariableCreatorId,
        attributeProductVariableStatusId
    FROM [SQM_GENERAL].[Tbl_AttributeProductVariables] (NOLOCK);
END;
GO

EXEC [SQM_GENERAL].[sp_AttributeProductVariables_List];