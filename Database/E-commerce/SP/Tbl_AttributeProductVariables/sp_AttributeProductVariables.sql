
USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_Create]
(
    @attributeProductVariableProductVariableId INT,
    @attributeProductVariableAttributeProductId INT,
    @attributeProductVariableValue VARCHAR(50),
    @attributeProductVariableCreatorId INT,
    @attributeProductVariableModificatorId INT = NULL,
    @attributeProductVariableStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO [SQM_GENERAL].[Tbl_AttributeProductVariables] 
            (attributeProductVariableProductVariableId, attributeProductVariableAttributeProductId, attributeProductVariableValue, attributeProductVariableCreatorId, attributeProductVariableCreationDate, attributeProductVariableModificatorId, attributeProductVariableModificationDate, attributeProductVariableStatusId)
        VALUES 
            (@attributeProductVariableProductVariableId, @attributeProductVariableAttributeProductId, TRIM(@attributeProductVariableValue), @attributeProductVariableCreatorId, GETDATE(), @attributeProductVariableModificatorId, NULL, @attributeProductVariableStatusId);
        
        SET @o_templateId = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro creado exitosamente.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_Update]
(
    @attributeProductVariableId INT,
    @attributeProductVariableProductVariableId INT,
    @attributeProductVariableAttributeProductId INT,
    @attributeProductVariableValue VARCHAR(50),
    @attributeProductVariableCreatorId INT,
    @attributeProductVariableModificatorId INT,
    @attributeProductVariableStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE [SQM_GENERAL].[Tbl_AttributeProductVariables]
        SET attributeProductVariableProductVariableId = @attributeProductVariableProductVariableId,
            attributeProductVariableAttributeProductId = @attributeProductVariableAttributeProductId,
            attributeProductVariableValue = TRIM(@attributeProductVariableValue),
            attributeProductVariableCreatorId = @attributeProductVariableCreatorId,
            attributeProductVariableModificatorId = @attributeProductVariableModificatorId,
            attributeProductVariableModificationDate = GETDATE(),
            attributeProductVariableStatusId = @attributeProductVariableStatusId
        WHERE attributeProductVariableId = @attributeProductVariableId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado exitosamente.'; SET @o_templateId = @attributeProductVariableId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_Delete]
(
    @attributeProductVariableId INT,
    @attributeProductVariableProductVariableId INT,
    @attributeProductVariableAttributeProductId INT,
    @attributeProductVariableValue VARCHAR(50),
    @attributeProductVariableCreatorId INT,
    @attributeProductVariableModificatorId INT,
    @attributeProductVariableStatusId BIT,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_templateId INT = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE [SQM_GENERAL].[Tbl_AttributeProductVariables] 
        SET attributeProductVariableProductVariableId = @attributeProductVariableProductVariableId,
            attributeProductVariableAttributeProductId = @attributeProductVariableAttributeProductId,
            attributeProductVariableValue = TRIM(@attributeProductVariableValue),
            attributeProductVariableCreatorId = @attributeProductVariableCreatorId,
            attributeProductVariableModificatorId = @attributeProductVariableModificatorId, 
            attributeProductVariableModificationDate = GETDATE(),
            attributeProductVariableStatusId = @attributeProductVariableStatusId
        WHERE attributeProductVariableId = @attributeProductVariableId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado/deshabilitado.'; SET @o_templateId = @attributeProductVariableId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_Filter]
    @attributeProductVariableId INT = NULL,
    @attributeProductVariableProductVariableId INT = NULL,
    @attributeProductVariableAttributeProductId INT = NULL,
    @attributeProductVariableValue VARCHAR(50) = NULL,
    @attributeProductVariableCreatorId INT = NULL,
    @attributeProductVariableCreationDate DATETIME = NULL,
    @attributeProductVariableModificatorId INT = NULL,
    @attributeProductVariableModificationDate DATETIME = NULL,
    @attributeProductVariableStatusId BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT attributeProductVariableId, attributeProductVariableProductVariableId, attributeProductVariableAttributeProductId, attributeProductVariableValue, attributeProductVariableCreatorId, attributeProductVariableCreationDate, attributeProductVariableModificatorId, attributeProductVariableModificationDate, attributeProductVariableStatusId 
    FROM [SQM_GENERAL].[Tbl_AttributeProductVariables] (NOLOCK)
    WHERE (@attributeProductVariableId IS NULL OR attributeProductVariableId = @attributeProductVariableId)
      AND (@attributeProductVariableProductVariableId IS NULL OR attributeProductVariableProductVariableId = @attributeProductVariableProductVariableId)
      AND (@attributeProductVariableAttributeProductId IS NULL OR attributeProductVariableAttributeProductId = @attributeProductVariableAttributeProductId)
      AND (@attributeProductVariableValue IS NULL OR attributeProductVariableValue LIKE '%' + TRIM(@attributeProductVariableValue) + '%')
      AND (@attributeProductVariableCreatorId IS NULL OR attributeProductVariableCreatorId = @attributeProductVariableCreatorId)
      AND (@attributeProductVariableCreationDate IS NULL OR CAST(attributeProductVariableCreationDate AS DATE) = CAST(attributeProductVariableCreationDate AS DATE))
      AND (@attributeProductVariableModificatorId IS NULL OR attributeProductVariableModificatorId = @attributeProductVariableModificatorId)
      AND (@attributeProductVariableModificationDate IS NULL OR CAST(attributeProductVariableModificationDate AS DATE) = CAST(attributeProductVariableModificationDate AS DATE))
      AND (@attributeProductVariableStatusId IS NULL OR attributeProductVariableStatusId = @attributeProductVariableStatusId)
    OPTION (RECOMPILE);
END;
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_AttributeProductVariables_List]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT attributeProductVariableId, attributeProductVariableProductVariableId, attributeProductVariableAttributeProductId, attributeProductVariableValue, attributeProductVariableCreatorId, attributeProductVariableCreationDate, attributeProductVariableModificatorId, attributeProductVariableModificationDate, attributeProductVariableStatusId 
    FROM [SQM_GENERAL].[Tbl_AttributeProductVariables] (NOLOCK);
END;
GO