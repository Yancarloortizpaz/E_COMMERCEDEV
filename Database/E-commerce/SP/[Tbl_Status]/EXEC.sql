DECLARE @o_code INT,
        @o_message VARCHAR(255),
        @o_templateId INT;


-- CREATE

EXEC [SQM_CATALOGS].[sp_Status_Create]
    @statusName = 'Activo',
    @statusCreatorId = 1,
    @statusStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- UPDATE

EXEC [SQM_CATALOGS].[sp_Status_Update]
    @statusId = 1,
    @statusName = 'Activo Modificado',
    @statusCreatorId = 1,
    @statusStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;

-- DELETE (LÓGICO)

EXEC [SQM_CATALOGS].[sp_Status_Delete]
    @statusId = 1,
    @statusCreatorId = 1,
    @statusStatusId = 0,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- FILTER

EXEC [SQM_CATALOGS].[sp_Status_Filter]
    @statusId = 1;


-- LIST

EXEC [SQM_CATALOGS].[sp_Status_List];
GO