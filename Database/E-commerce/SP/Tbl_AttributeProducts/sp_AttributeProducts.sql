USE [DB_ECOMMERCE]
GO



CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributeProducts_Create]
(
    @AttributeProductAttributesTypeId INT,
    @AttributeProductName VARCHAR(50),
    @AttributeProductDescription VARCHAR(100),
    @AttributeProductCreatorId INT,
    @AttributeProductModificatorId INT = NULL,
    @AttributeProductStatusId BIT,
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
        INSERT INTO [SQM_CATALOGS].[Tbl_AttributeProducts] 
            (AttributeProductAttributesTypeId, AttributeProductName, AttributeProductDescription, AttributeProductCreatorId, AttributeProductCreationDate, AttributeProductModificatorId, AttributeProductModificationDate, AttributeProductStatusId)
        VALUES 
            (@AttributeProductAttributesTypeId, TRIM(@AttributeProductName), TRIM(@AttributeProductDescription), @AttributeProductCreatorId, GETDATE(), @AttributeProductModificatorId, NULL, @AttributeProductStatusId);
        
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

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributeProducts_Update]
(
    @AttributeProductId INT,
    @AttributeProductAttributesTypeId INT,
    @AttributeProductName VARCHAR(50),
    @AttributeProductDescription VARCHAR(100),
    @AttributeProductCreatorId INT,
    @AttributeProductModificatorId INT,
    @AttributeProductStatusId BIT,
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
        UPDATE [SQM_CATALOGS].[Tbl_AttributeProducts]
        SET AttributeProductAttributesTypeId = @AttributeProductAttributesTypeId,
            AttributeProductName = TRIM(@AttributeProductName),
            AttributeProductDescription = TRIM(@AttributeProductDescription),
            AttributeProductCreatorId = @AttributeProductCreatorId,
            AttributeProductModificatorId = @AttributeProductModificatorId,
            AttributeProductModificationDate = GETDATE(),
            AttributeProductStatusId = @AttributeProductStatusId
        WHERE AttributeProductId = @AttributeProductId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado exitosamente.'; SET @o_templateId = @AttributeProductId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributeProducts_Delete]
(
    @AttributeProductId INT,
    @AttributeProductAttributesTypeId INT,
    @AttributeProductName VARCHAR(50),
    @AttributeProductDescription VARCHAR(100),
    @AttributeProductCreatorId INT,
    @AttributeProductModificatorId INT,
    @AttributeProductStatusId BIT,
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
        UPDATE [SQM_CATALOGS].[Tbl_AttributeProducts] 
        SET AttributeProductAttributesTypeId = @AttributeProductAttributesTypeId,
            AttributeProductName = TRIM(@AttributeProductName),
            AttributeProductDescription = TRIM(@AttributeProductDescription),
            AttributeProductCreatorId = @AttributeProductCreatorId,
            AttributeProductModificatorId = @AttributeProductModificatorId, 
            AttributeProductModificationDate = GETDATE(),
            AttributeProductStatusId = @AttributeProductStatusId
        WHERE AttributeProductId = @AttributeProductId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado/deshabilitado.'; SET @o_templateId = @AttributeProductId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributeProducts_Filter]
    @AttributeProductId INT = NULL,
    @AttributeProductAttributesTypeId INT = NULL,
    @AttributeProductName VARCHAR(50) = NULL,
    @AttributeProductDescription VARCHAR(100) = NULL,
    @AttributeProductCreatorId INT = NULL,
    @AttributeProductCreationDate DATETIME = NULL,
    @AttributeProductModificatorId INT = NULL,
    @AttributeProductModificationDate DATETIME = NULL,
    @AttributeProductStatusId BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AttributeProductId, AttributeProductAttributesTypeId, AttributeProductName, AttributeProductDescription, AttributeProductCreatorId, AttributeProductCreationDate, AttributeProductModificatorId, AttributeProductModificationDate, AttributeProductStatusId 
    FROM [SQM_CATALOGS].[Tbl_AttributeProducts] (NOLOCK)
    WHERE (@AttributeProductId IS NULL OR AttributeProductId = @AttributeProductId)
      AND (@AttributeProductAttributesTypeId IS NULL OR AttributeProductAttributesTypeId = @AttributeProductAttributesTypeId)
      AND (@AttributeProductName IS NULL OR AttributeProductName LIKE '%' + TRIM(@AttributeProductName) + '%')
      AND (@AttributeProductDescription IS NULL OR AttributeProductDescription LIKE '%' + TRIM(@AttributeProductDescription) + '%')
      AND (@AttributeProductCreatorId IS NULL OR AttributeProductCreatorId = @AttributeProductCreatorId)
      AND (@AttributeProductCreationDate IS NULL OR CAST(AttributeProductCreationDate AS DATE) = CAST(AttributeProductCreationDate AS DATE))
      AND (@AttributeProductModificatorId IS NULL OR AttributeProductModificatorId = @AttributeProductModificatorId)
      AND (@AttributeProductModificationDate IS NULL OR CAST(AttributeProductModificationDate AS DATE) = CAST(AttributeProductModificationDate AS DATE))
      AND (@AttributeProductStatusId IS NULL OR AttributeProductStatusId = @AttributeProductStatusId)
    OPTION (RECOMPILE);
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributeProducts_List]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AttributeProductId, AttributeProductAttributesTypeId, AttributeProductName, AttributeProductDescription, AttributeProductCreatorId, AttributeProductCreationDate, AttributeProductModificatorId, AttributeProductModificationDate, AttributeProductStatusId 
    FROM [SQM_CATALOGS].[Tbl_AttributeProducts] (NOLOCK);
END;
GO