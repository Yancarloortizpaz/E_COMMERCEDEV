DECLARE @o_code INT,
        @o_message VARCHAR(255),
        @o_templateId INT;

-- CREATE

EXEC [SQM_GENERAL].[sp_ProductImages_Create]
    @productImageProductId = 1,
    @productImageURL = 'https://miweb.com/images/producto1.jpg',
    @productImageDescription = 'Imagen principal del producto',
    @productImageIsPrincipal = 1,
    @productImageCreatorId = 1,
    @productImageStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;

-- UPDATE

EXEC [SQM_GENERAL].[sp_ProductImages_Update]
    @productImageId = 1,
    @productImageProductId = 1,
    @productImageURL = 'https://miweb.com/images/producto1_nueva.jpg',
    @productImageDescription = 'Imagen principal actualizada',
    @productImageIsPrincipal = 1,
    @productImageModificatorId = 1,
    @productImageStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;

-- DELETE 
EXEC [SQM_GENERAL].[sp_ProductImages_Delete]
    @productImageId = 1,
    @productImageModificatorId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- FILTER

EXEC [SQM_GENERAL].[sp_ProductImages_Filter]
    @productImageId = 1;

EXEC [SQM_GENERAL].[sp_ProductImages_Filter]
    @productImageProductId = 1;

EXEC [SQM_GENERAL].[sp_ProductImages_Filter]
    @productImageIsPrincipal = 1;

-- LIST

EXEC [SQM_GENERAL].[sp_ProductImages_List];
GO