USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductVariableTypes_Create]
(
    @productVariableTypeName VARCHAR(50),
    @productVariableTypeDescription VARCHAR(100),
    @productVariableTypeCreatorId INT,
    @productVariableTypeModificatorId INT = NULL,
    @productVariableTypeStatusId BIT,
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
        INSERT INTO [SQM_CATALOGS].[Tbl_ProductVariableTypes] 
            (productVariableTypeName, productVariableTypeDescription, productVariableTypeCreatorId, productVariableTypeCreationDate, productVariableTypeModificatorId, productVariableTypeModificationDate, productVariableTypeStatusId)
        VALUES 
            (TRIM(@productVariableTypeName), TRIM(@productVariableTypeDescription), @productVariableTypeCreatorId, GETDATE(), @productVariableTypeModificatorId, NULL, @productVariableTypeStatusId);
        
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

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductVariableTypes_Update]
(
    @productVariableTypeId INT,
    @productVariableTypeName VARCHAR(50),
    @productVariableTypeDescription VARCHAR(100),
    @productVariableTypeCreatorId INT,
    @productVariableTypeModificatorId INT,
    @productVariableTypeStatusId BIT,
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
        UPDATE [SQM_CATALOGS].[Tbl_ProductVariableTypes]
        SET productVariableTypeName = TRIM(@productVariableTypeName),
            productVariableTypeDescription = TRIM(@productVariableTypeDescription),
            productVariableTypeCreatorId = @productVariableTypeCreatorId,
            productVariableTypeModificatorId = @productVariableTypeModificatorId,
            productVariableTypeModificationDate = GETDATE(),
            productVariableTypeStatusId = @productVariableTypeStatusId
        WHERE productVariableTypeId = @productVariableTypeId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado exitosamente.'; SET @o_templateId = @productVariableTypeId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductVariableTypes_Delete]
(
    @productVariableTypeId INT,
    @productVariableTypeName VARCHAR(50),
    @productVariableTypeDescription VARCHAR(100),
    @productVariableTypeCreatorId INT,
    @productVariableTypeModificatorId INT,
    @productVariableTypeStatusId BIT,
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
        UPDATE [SQM_CATALOGS].[Tbl_ProductVariableTypes] 
        SET productVariableTypeName = TRIM(@productVariableTypeName),
            productVariableTypeDescription = TRIM(@productVariableTypeDescription),
            productVariableTypeCreatorId = @productVariableTypeCreatorId,
            productVariableTypeModificatorId = @productVariableTypeModificatorId, 
            productVariableTypeModificationDate = GETDATE(),
            productVariableTypeStatusId = @productVariableTypeStatusId
        WHERE productVariableTypeId = @productVariableTypeId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado/deshabilitado.'; SET @o_templateId = @productVariableTypeId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductVariableTypes_Filter]
    @productVariableTypeId INT = NULL,
    @productVariableTypeName VARCHAR(50) = NULL,
    @productVariableTypeDescription VARCHAR(100) = NULL,
    @productVariableTypeCreatorId INT = NULL,
    @productVariableTypeCreationDate DATETIME = NULL,
    @productVariableTypeModificatorId INT = NULL,
    @productVariableTypeModificationDate DATETIME = NULL,
    @productVariableTypeStatusId BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT productVariableTypeId, productVariableTypeName, productVariableTypeDescription, productVariableTypeCreatorId, productVariableTypeCreationDate, productVariableTypeModificatorId, productVariableTypeModificationDate, productVariableTypeStatusId 
    FROM [SQM_CATALOGS].[Tbl_ProductVariableTypes] (NOLOCK)
    WHERE (@productVariableTypeId IS NULL OR productVariableTypeId = @productVariableTypeId)
      AND (@productVariableTypeName IS NULL OR productVariableTypeName LIKE '%' + TRIM(@productVariableTypeName) + '%')
      AND (@productVariableTypeDescription IS NULL OR productVariableTypeDescription LIKE '%' + TRIM(@productVariableTypeDescription) + '%')
      AND (@productVariableTypeCreatorId IS NULL OR productVariableTypeCreatorId = @productVariableTypeCreatorId)
      AND (@productVariableTypeCreationDate IS NULL OR CAST(productVariableTypeCreationDate AS DATE) = CAST(productVariableTypeCreationDate AS DATE))
      AND (@productVariableTypeModificatorId IS NULL OR productVariableTypeModificatorId = @productVariableTypeModificatorId)
      AND (@productVariableTypeModificationDate IS NULL OR CAST(productVariableTypeModificationDate AS DATE) = CAST(productVariableTypeModificationDate AS DATE))
      AND (@productVariableTypeStatusId IS NULL OR productVariableTypeStatusId = @productVariableTypeStatusId)
    OPTION (RECOMPILE);
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductVariableTypes_List]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT productVariableTypeId, productVariableTypeName, productVariableTypeDescription, productVariableTypeCreatorId, productVariableTypeCreationDate, productVariableTypeModificatorId, productVariableTypeModificationDate, productVariableTypeStatusId 
    FROM [SQM_CATALOGS].[Tbl_ProductVariableTypes] (NOLOCK);
END;
GO