DECLARE @o_code INT,
        @o_message VARCHAR(255),
        @o_templateId INT;


-- CREATE

EXEC [SQM_CATALOGS].[sp_MarkByProviders_Create]
    @markByProviderMarkId = 1,
    @markByProviderProviderId = 1,
    @markByProviderCreatorId = 1,
    @markByProviderStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- UPDATE

EXEC [SQM_CATALOGS].[sp_MarkByProviders_Update]
    @markByProviderId = 1,
    @markByProviderMarkId = 1,
    @markByProviderProviderId = 1,
    @markByProviderModificatorId = 1,
    @markByProviderStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- DELETE

EXEC [SQM_CATALOGS].[sp_MarkByProviders_Delete]
    @markByProviderId = 1,
    @markByProviderModificatorId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- FILTER

EXEC [SQM_CATALOGS].[sp_MarkByProviders_Filter]
    @markByProviderId = 1;

EXEC [SQM_CATALOGS].[sp_MarkByProviders_Filter]
    @markByProviderMarkId = 1;

EXEC [SQM_CATALOGS].[sp_MarkByProviders_Filter]
    @markByProviderProviderId = 1;

-- LIST

EXEC [SQM_CATALOGS].[sp_MarkByProviders_List];
GO