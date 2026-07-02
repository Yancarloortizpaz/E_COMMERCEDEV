DECLARE @o_code INT,
        @o_message VARCHAR(255),
        @o_templateId INT;


-- CREATE

EXEC [SQM_CATALOGS].[sp_AttributeProducts_Create]
    @AttributeProductAttributesTypeId = 1,
    @AttributeProductName = 'Color',
    @AttributeProductDescription = 'Color del producto',
    @AttributeProductCreatorId = 1,
    @AttributeProductStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- UPDATE

EXEC [SQM_CATALOGS].[sp_AttributeProducts_Update]
    @AttributeProductId = 1,
    @AttributeProductAttributesTypeId = 1,
    @AttributeProductName = 'Color Modificado',
    @AttributeProductDescription = 'Descripción modificada',
    @AttributeProductModificatorId = 1,
    @AttributeProductStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;

-- DELETE 
EXEC [SQM_CATALOGS].[sp_AttributeProducts_Delete]
    @AttributeProductId = 1,
    @AttributeProductModificatorId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;

-- FILTER

EXEC [SQM_CATALOGS].[sp_AttributeProducts_Filter]
    @AttributeProductId = 1;

EXEC [SQM_CATALOGS].[sp_AttributeProducts_Filter]
    @AttributeProductName = 'Color';

EXEC [SQM_CATALOGS].[sp_AttributeProducts_Filter]
    @AttributeProductAttributesTypeId = 1;


-- LIST

EXEC [SQM_CATALOGS].[sp_AttributeProducts_List];
GO