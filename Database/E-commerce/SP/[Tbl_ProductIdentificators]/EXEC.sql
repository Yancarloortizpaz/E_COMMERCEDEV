DECLARE @o_code INT,
        @o_message VARCHAR(255),
        @o_templateId INT;


-- CREATE

EXEC [SQM_CATALOGS].[sp_ProductIdentificators_Create]
    @productIdentificatorCategoryId = 1,
    @productIdentificatorSubCategoryId = 1,
    @productIdentificatorSegmentId = 1,
    @productIdentificatorCreatorId = 1,
    @productIdentificatorStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- UPDATE

EXEC [SQM_CATALOGS].[sp_ProductIdentificators_Update]
    @productIdentificatorId = 1,
    @productIdentificatorCategoryId = 1,
    @productIdentificatorSubCategoryId = 1,
    @productIdentificatorSegmentId = 1,
    @productIdentificatorModificatorId = 1,
    @productIdentificatorStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- DELETE 

EXEC [SQM_CATALOGS].[sp_ProductIdentificators_Delete]
    @productIdentificatorId = 1,
    @productIdentificatorModificatorId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- FILTER

EXEC [SQM_CATALOGS].[sp_ProductIdentificators_Filter]
    @productIdentificatorId = 1;


-- LIST

EXEC [SQM_CATALOGS].[sp_ProductIdentificators_List];
GO