CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributeProducts_Filter]
(
    @AttributeProductId INT = NULL,
    @AttributeProductAttributesTypeId INT = NULL,
    @AttributeProductName VARCHAR(50) = NULL,
    @AttributeProductDescription VARCHAR(100) = NULL,
    @AttributeProductCreatorId INT = NULL,
    @AttributeProductCreationDate DATETIME = NULL,
    @AttributeProductModificatorId INT = NULL,
    @AttributeProductModificationDate DATETIME = NULL,
    @AttributeProductStatusId BIT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        AttributeProductId,
        AttributeProductAttributesTypeId,
        AttributeProductName,
        AttributeProductDescription,
        AttributeProductCreatorId,
        AttributeProductCreationDate,
        AttributeProductModificatorId,
        AttributeProductModificationDate,
        AttributeProductStatusId
    FROM [SQM_CATALOGS].[Tbl_AttributeProducts] (NOLOCK)
    WHERE
        (@AttributeProductId IS NULL OR AttributeProductId = @AttributeProductId)
        AND (@AttributeProductAttributesTypeId IS NULL OR AttributeProductAttributesTypeId = @AttributeProductAttributesTypeId)
        AND (@AttributeProductName IS NULL OR AttributeProductName LIKE '%' + LTRIM(RTRIM(@AttributeProductName)) + '%')
        AND (@AttributeProductDescription IS NULL OR AttributeProductDescription LIKE '%' + LTRIM(RTRIM(@AttributeProductDescription)) + '%')
        AND (@AttributeProductCreatorId IS NULL OR AttributeProductCreatorId = @AttributeProductCreatorId)
        AND (@AttributeProductCreationDate IS NULL OR CAST(AttributeProductCreationDate AS DATE) = CAST(@AttributeProductCreationDate AS DATE))
        AND (@AttributeProductModificatorId IS NULL OR AttributeProductModificatorId = @AttributeProductModificatorId)
        AND (@AttributeProductModificationDate IS NULL OR CAST(AttributeProductModificationDate AS DATE) = CAST(@AttributeProductModificationDate AS DATE))
        AND (@AttributeProductStatusId IS NULL OR AttributeProductStatusId = @AttributeProductStatusId)
    OPTION (RECOMPILE);
END;
GO