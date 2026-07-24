CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_Update]
(
    @attributeProductVariableId INT,
    @attributeProductVariableProductVariableId INT,
    @attributeProductVariableAttributeProductId INT,
    @attributeProductVariableValue VARCHAR(50),
    @attributeProductVariableModificatorId INT,
    @attributeProductVariableStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_Delete]
(
    @attributeProductVariableId INT,
    @attributeProductVariableModificatorId INT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS




CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_Filter]
(
    @attributeProductVariableId INT = NULL,
    @attributeProductVariableProductVariableId INT = NULL,
    @attributeProductVariableAttributeProductId INT = NULL,
    @attributeProductVariableValue VARCHAR(50) = NULL,
    @attributeProductVariableCreatorId INT = NULL,
    @attributeProductVariableCreationDate DATETIME = NULL,
    @attributeProductVariableModificatorId INT = NULL,
    @attributeProductVariableModificationDate DATETIME = NULL,
    @attributeProductVariableStatusId BIT = NULL
)
AS BEGIN

    SELECT attributeProductVariableId, attributeProductVariableProductVariableId, attributeProductVariableAttributeProductId, attributeProductVariableValue, attributeProductVariableCreatorId, attributeProductVariableCreationDate, attributeProductVariableModificatorId, attributeProductVariableModificationDate, attributeProductVariableStatusId


    CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_Create]
(
    @attributeProductVariableProductVariableId INT,
    @attributeProductVariableAttributeProductId INT,
    @attributeProductVariableValue NVARCHAR(50),
    @attributeProductVariableCreatorId INT,
    @attributeProductVariableStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message NVARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_List]
AS BEGIN
    SELECT attributeProductVariableId, attributeProductVariableProductVariableId, attributeProductVariableAttributeProductId, attributeProductVariableValue, attributeProductVariableCreatorId, attributeProductVariableStatusId