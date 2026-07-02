DECLARE @o_code INT,
        @o_message VARCHAR(255),
        @o_templateId INT;


-- CREATE

EXEC [SQM_GENERAL].[sp_AttributeProductVariables_Create]
    @attributeProductVariableProductVariableId = 1,
    @attributeProductVariableAttributeProductId = 1,
    @attributeProductVariableValue = 'Rojo',
    @attributeProductVariableCreatorId = 1,
    @attributeProductVariableStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- UPDATE

EXEC [SQM_GENERAL].[sp_AttributeProductVariables_Update]
    @attributeProductVariableId = 1,
    @attributeProductVariableProductVariableId = 1,
    @attributeProductVariableAttributeProductId = 1,
    @attributeProductVariableValue = 'Azul',
    @attributeProductVariableModificatorId = 1,
    @attributeProductVariableStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- DELETE 
EXEC [SQM_GENERAL].[sp_AttributeProductVariables_Delete]
    @attributeProductVariableId = 1,
    @attributeProductVariableModificatorId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- FILTER

EXEC [SQM_GENERAL].[sp_AttributeProductVariables_Filter]
    @attributeProductVariableId = 1;

EXEC [SQM_GENERAL].[sp_AttributeProductVariables_Filter]
    @attributeProductVariableProductVariableId = 1;

EXEC [SQM_GENERAL].[sp_AttributeProductVariables_Filter]
    @attributeProductVariableAttributeProductId = 1;

EXEC [SQM_GENERAL].[sp_AttributeProductVariables_Filter]
    @attributeProductVariableStatusId = 1;

-- LIST

EXEC [SQM_GENERAL].[sp_AttributeProductVariables_List];
GO