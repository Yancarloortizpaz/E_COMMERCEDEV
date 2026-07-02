DECLARE @o_code INT,
        @o_message VARCHAR(255),
        @o_templateId INT;


-- CREATE

EXEC [SQM_CATALOGS].[sp_StockMovementTypes_Create]
    @stockMovementTypeName = 'Entrada',
    @stockMovementTypeDescription = 'Ingreso de inventario',
    @stockMovementTypeCreatorId = 1,
    @stockMovementTypeStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- UPDATE

EXEC [SQM_CATALOGS].[sp_StockMovementTypes_Update]
    @stockMovementTypeId = 1,
    @stockMovementTypeName = 'Salida',
    @stockMovementTypeDescription = 'Salida de inventario',
    @stockMovementTypeModificatorId = 1,
    @stockMovementTypeStatusId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- DELETE 

EXEC [SQM_CATALOGS].[sp_StockMovementTypes_Delete]
    @stockMovementTypeId = 1,
    @stockMovementTypeModificatorId = 1,
    @o_code = @o_code OUTPUT,
    @o_message = @o_message OUTPUT,
    @o_templateId = @o_templateId OUTPUT;

SELECT
    @o_code AS Code,
    @o_message AS Message,
    @o_templateId AS TemplateId;


-- FILTER

EXEC [SQM_CATALOGS].[sp_StockMovementTypes_Filter]
    @stockMovementTypeId = 1;

EXEC [SQM_CATALOGS].[sp_StockMovementTypes_Filter]
    @stockMovementTypeName = 'Entrada';

EXEC [SQM_CATALOGS].[sp_StockMovementTypes_Filter]
    @stockMovementTypeStatusId = 1;


-- LIST

EXEC [SQM_CATALOGS].[sp_StockMovementTypes_List];
GO