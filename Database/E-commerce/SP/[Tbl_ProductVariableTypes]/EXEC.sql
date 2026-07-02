DECLARE @o_code INT,
        @o_message VARCHAR(255),
        @o_templateId INT;

-- CREATE

EXEC [SQM_CATALOGS].[sp_ProductVariableTypes_Create]
    @productVariableTypeName = 'Tamaño',
    @productVariableTypeDescription = 'Tipos de tamaños del producto',
    @productVariableTypeCreatorId = 1,
    @productVariableTypeStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- UPDATE

EXEC [SQM_CATALOGS].[sp_ProductVariableTypes_Update]
    @productVariableTypeId = 1,
    @productVariableTypeName = 'Tamaño Modificado',
    @productVariableTypeDescription = 'Descripción modificada',
    @productVariableTypeModificatorId = 1,
    @productVariableTypeStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- DELETE 
EXEC [SQM_CATALOGS].[sp_ProductVariableTypes_Delete]
    @productVariableTypeId = 1,
    @productVariableTypeModificatorId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;

-- FILTER

EXEC [SQM_CATALOGS].[sp_ProductVariableTypes_Filter]
    @productVariableTypeId = 1;

EXEC [SQM_CATALOGS].[sp_ProductVariableTypes_Filter]
    @productVariableTypeName = 'Tamaño';

EXEC [SQM_CATALOGS].[sp_ProductVariableTypes_Filter]
    @productVariableTypeStatusId = 1;


-- LIST

EXEC [SQM_CATALOGS].[sp_ProductVariableTypes_List];
GO