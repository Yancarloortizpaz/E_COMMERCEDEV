DECLARE @o_code INT,
        @o_message VARCHAR(255),
        @o_templateId INT;

-- CREATE

EXEC [SQM_CATALOGS].[sp_AttributesTypes_Create]
    @attributeTypeName = 'Color',
    @attributeTypeDescription = 'Atributos de color',
    @attributeTypeCreatorId = 1,
    @attributeTypeStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- UPDATE

EXEC [SQM_CATALOGS].[sp_AttributesTypes_Update]
    @attributeTypeId = 1,
    @attributeTypeName = 'Color Modificado',
    @attributeTypeDescription = 'Atributos de color modificados',
    @attributeTypeModificatorId = 1,
    @attributeTypeStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- DELETE 

EXEC [SQM_CATALOGS].[sp_AttributesTypes_Delete]
    @attributeTypeId = 1,
    @attributeTypeModificatorId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- FILTER

EXEC [SQM_CATALOGS].[sp_AttributesTypes_Filter]
    @attributeTypeId = 1;

EXEC [SQM_CATALOGS].[sp_AttributesTypes_Filter]
    @attributeTypeName = 'Color';


-- LIST

EXEC [SQM_CATALOGS].[sp_AttributesTypes_List];
GO