USE [DB_ECOMMERCE]
GO


CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributesTypes_Create]
(
    @attributeTypeName VARCHAR(50),
    @attributeTypeDescription VARCHAR(100),
    @attributeTypeCreatorId INT,
    @attributeTypeModificatorId INT = NULL,
    @attributeTypeStatusId BIT,
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
        INSERT INTO [SQM_CATALOGS].[Tbl_AttributesTypes] 
            (attributeTypeName, attributeTypeDescription, attributeTypeCreatorId, attributeTypeCreationDate, attributeTypeModificatorId, attributeTypeModificationDate, attributeTypeStatusId)
        VALUES 
            (TRIM(@attributeTypeName), TRIM(@attributeTypeDescription), @attributeTypeCreatorId, GETDATE(), @attributeTypeModificatorId, NULL, @attributeTypeStatusId);
        
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

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributesTypes_Update]
(
    @attributeTypeId INT,
    @attributeTypeName VARCHAR(50),
    @attributeTypeDescription VARCHAR(100),
    @attributeTypeCreatorId INT,
    @attributeTypeModificatorId INT,
    @attributeTypeStatusId BIT,
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
        UPDATE [SQM_CATALOGS].[Tbl_AttributesTypes]
        SET attributeTypeName = TRIM(@attributeTypeName),
            attributeTypeDescription = TRIM(@attributeTypeDescription),
            attributeTypeCreatorId = @attributeTypeCreatorId,
            attributeTypeModificatorId = @attributeTypeModificatorId,
            attributeTypeModificationDate = GETDATE(),
            attributeTypeStatusId = @attributeTypeStatusId
        WHERE attributeTypeId = @attributeTypeId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado exitosamente.'; SET @o_templateId = @attributeTypeId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributesTypes_Delete]
(
    @attributeTypeId INT,
    @attributeTypeName VARCHAR(50),
    @attributeTypeDescription VARCHAR(100),
    @attributeTypeCreatorId INT,
    @attributeTypeModificatorId INT,
    @attributeTypeStatusId BIT,
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
        UPDATE [SQM_CATALOGS].[Tbl_AttributesTypes] 
        SET attributeTypeName = TRIM(@attributeTypeName),
            attributeTypeDescription = TRIM(@attributeTypeDescription),
            attributeTypeCreatorId = @attributeTypeCreatorId,
            attributeTypeModificatorId = @attributeTypeModificatorId, 
            attributeTypeModificationDate = GETDATE(),
            attributeTypeStatusId = @attributeTypeStatusId
        WHERE attributeTypeId = @attributeTypeId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado/deshabilitado.'; SET @o_templateId = @attributeTypeId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributesTypes_Filter]
    @attributeTypeId INT = NULL,
    @attributeTypeName VARCHAR(50) = NULL,
    @attributeTypeDescription VARCHAR(100) = NULL,
    @attributeTypeCreatorId INT = NULL,
    @attributeTypeCreationDate DATETIME = NULL,
    @attributeTypeModificatorId INT = NULL,
    @attributeTypeModificationDate DATETIME = NULL,
    @attributeTypeStatusId BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT attributeTypeId, attributeTypeName, attributeTypeDescription, attributeTypeCreatorId, attributeTypeCreationDate, attributeTypeModificatorId, attributeTypeModificationDate, attributeTypeStatusId 
    FROM [SQM_CATALOGS].[Tbl_AttributesTypes] (NOLOCK)
    WHERE (@attributeTypeId IS NULL OR attributeTypeId = @attributeTypeId)
      AND (@attributeTypeName IS NULL OR attributeTypeName LIKE '%' + TRIM(@attributeTypeName) + '%')
      AND (@attributeTypeDescription IS NULL OR attributeTypeDescription LIKE '%' + TRIM(@attributeTypeDescription) + '%')
      AND (@attributeTypeCreatorId IS NULL OR attributeTypeCreatorId = @attributeTypeCreatorId)
      AND (@attributeTypeCreationDate IS NULL OR CAST(attributeTypeCreationDate AS DATE) = CAST(@attributeTypeCreationDate AS DATE))
      AND (@attributeTypeModificatorId IS NULL OR attributeTypeModificatorId = @attributeTypeModificatorId)
      AND (@attributeTypeModificationDate IS NULL OR CAST(attributeTypeModificationDate AS DATE) = CAST(@attributeTypeModificationDate AS DATE))
      AND (@attributeTypeStatusId IS NULL OR attributeTypeStatusId = @attributeTypeStatusId)
    OPTION (RECOMPILE);
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributesTypes_List]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT attributeTypeId, attributeTypeName, attributeTypeDescription, attributeTypeCreatorId, attributeTypeCreationDate, attributeTypeModificatorId, attributeTypeModificationDate, attributeTypeStatusId 
    FROM [SQM_CATALOGS].[Tbl_AttributesTypes] (NOLOCK);
END;
GO