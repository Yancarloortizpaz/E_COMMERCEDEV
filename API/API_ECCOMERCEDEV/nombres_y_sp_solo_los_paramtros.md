CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductVariableTypes_Update]
(
    @productVariableTypeId INT,
    @productVariableTypeName VARCHAR(50),
    @productVariableTypeDescription VARCHAR(100),
    @productVariableTypeModificatorId INT,
    @productVariableTypeStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS[cite: 36]

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductVariableTypes_Delete]
(
    @productVariableTypeId INT,
    @productVariableTypeModificatorId INT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS[cite: 38]




CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductVariableTypes_Filter]
(
    @productVariableTypeId INT = NULL,
    @productVariableTypeName VARCHAR(50) = NULL,
    @productVariableTypeDescription VARCHAR(100) = NULL,
    @productVariableTypeCreatorId INT = NULL,
    @productVariableTypeCreationDate DATETIME = NULL,
    @productVariableTypeModificatorId INT = NULL,
    @productVariableTypeModificationDate DATETIME = NULL,
    @productVariableTypeStatusId BIT = NULL
)
AS BEGIN

    SELECT productVariableTypeId, productVariableTypeName, productVariableTypeDescription, productVariableTypeCreatorId, productVariableTypeCreationDate, productVariableTypeModificatorId, productVariableTypeModificationDate, productVariableTypeStatusId[cite: 39]


    CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductVariableTypes_Create]
(
    @productVariableTypeName VARCHAR(50),
    @productVariableTypeDescription VARCHAR(100),
    @productVariableTypeCreatorId INT,
    @productVariableTypeStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS[cite: 37]

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductVariableTypes_List]
AS BEGIN
    SELECT productVariableTypeId, productVariableTypeName, productVariableTypeDescription, productVariableTypeCreatorId, productVariableTypeStatusId[cite: 40]