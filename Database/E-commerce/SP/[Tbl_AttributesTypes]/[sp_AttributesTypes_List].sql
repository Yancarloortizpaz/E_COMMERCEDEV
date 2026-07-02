CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributesTypes_List]
AS
BEGIN
    SELECT
        attributeTypeId,
        attributeTypeName,
        attributeTypeDescription,
        attributeTypeCreatorId,
        attributeTypeStatusId
    FROM [SQM_CATALOGS].[Tbl_AttributesTypes] (NOLOCK);
END;
GO

EXEC [SQM_CATALOGS].[sp_AttributesTypes_List];