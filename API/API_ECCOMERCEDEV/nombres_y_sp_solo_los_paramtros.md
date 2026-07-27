CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Stocks_Delete]
(
    @stockId INT,
    @stockModificatorId INT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS



CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Stocks_Filter]
    @SearchTerm VARCHAR(50) = NULL, 
	@ProductVariableId INT = null
AS BEGIN
    DECLARE @SearchId INT = TRY_CAST(@SearchTerm AS INT);

    SELECT 
        stockId,
        productVariableId,
        productName,
        variableValue,
        unitPrice,
        currencyISO,
        quantity,
        factoryDate,
        expirationDate,
        statusId



        CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_GetByProductVariable]
(
    @ProductVariableId INT = NULL,
    @SearchTerm VARCHAR(100) = NULL
)
AS 
BEGIN
    SET NOCOUNT ON;

    DECLARE @TerminoBusqueda VARCHAR(102) = NULL;
    IF @SearchTerm IS NOT NULL AND RTRIM(LTRIM(@SearchTerm)) <> ''
    BEGIN
        SET @TerminoBusqueda = '%' + RTRIM(LTRIM(@SearchTerm)) + '%';
    END

    SELECT 
        IdAtributoVariable,
        ValorAtributo,
        RegistroActivo,
        IdTipoVariable,
        TipoVariable,
        DescripcionTipoVariable,
        IdVariante,
        ValorVariante,
        PrecioVariante,
        CodigoMoneda,
        NombreMoneda,
        IdProducto,
        NombreProducto,
        DescripcionProducto,
        NombreMarca,
        NombreProveedor,
        FechaCreacion,
        CreadoPor,
        FechaModificacion,
        ModificadoPor



        CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Stocks_List]
AS BEGIN
    SELECT 
        stockId,
        productVariableId,
        productName,
        variableValue,
        unitPrice,
        currencyISO,
        quantity,
        factoryDate,
        expirationDate,
        statusId


        CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Stocks_Create]
(
    @stockProductVariableId INT,
    @stockQuantity INT,
    @stockFactoryDate DATE,
    @stockExpirationDate DATE,
    @stockCreatorId INT,
    @stockStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS



CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Stocks_Update]
(
    @stockId INT,
    @stockQuantityAdjustment INT, -- Positivo para sumar, negativo para restar
    @stockModificatorId INT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS