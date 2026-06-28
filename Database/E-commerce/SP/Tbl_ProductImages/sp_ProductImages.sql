USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductImages_Create]
(
    @productImageProductId INT,
    @productImageURL VARCHAR(200),
    @productImageDescription VARCHAR(100),
    @productImageIsPrincipal BIT,
    @productImageCreatorId INT,
    @productImageModificatorId INT = NULL,
    @productImageStatusId BIT,
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
        INSERT INTO [SQM_GENERAL].[Tbl_ProductImages] 
            (productImageProductId, productImageURL, productImageDescription, productImageIsPrincipal, productImageCreatorId, productImageCreationDate, productImageModificatorId, productImageModificationDate, productImageStatusId)
        VALUES 
            (@productImageProductId, TRIM(@productImageURL), TRIM(@productImageDescription), @productImageIsPrincipal, @productImageCreatorId, GETDATE(), @productImageModificatorId, NULL, @productImageStatusId);
        
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

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductImages_Update]
(
    @productImageId INT,
    @productImageProductId INT,
    @productImageURL VARCHAR(200),
    @productImageDescription VARCHAR(100),
    @productImageIsPrincipal BIT,
    @productImageCreatorId INT,
    @productImageModificatorId INT,
    @productImageStatusId BIT,
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
        UPDATE [SQM_GENERAL].[Tbl_ProductImages]
        SET productImageProductId = @productImageProductId,
            productImageURL = TRIM(@productImageURL),
            productImageDescription = TRIM(@productImageDescription),
            productImageIsPrincipal = @productImageIsPrincipal,
            productImageCreatorId = @productImageCreatorId,
            productImageModificatorId = @productImageModificatorId,
            productImageModificationDate = GETDATE(),
            productImageStatusId = @productImageStatusId
        WHERE productImageId = @productImageId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado exitosamente.'; SET @o_templateId = @productImageId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductImages_Delete]
(
    @productImageId INT,
    @productImageProductId INT,
    @productImageURL VARCHAR(200),
    @productImageDescription VARCHAR(100),
    @productImageIsPrincipal BIT,
    @productImageCreatorId INT,
    @productImageModificatorId INT,
    @productImageStatusId BIT,
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
        UPDATE [SQM_GENERAL].[Tbl_ProductImages] 
        SET productImageProductId = @productImageProductId,
            productImageURL = TRIM(@productImageURL),
            productImageDescription = TRIM(@productImageDescription),
            productImageIsPrincipal = @productImageIsPrincipal,
            productImageCreatorId = @productImageCreatorId,
            productImageModificatorId = @productImageModificatorId, 
            productImageModificationDate = GETDATE(),
            productImageStatusId = @productImageStatusId
        WHERE productImageId = @productImageId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado/deshabilitado.'; SET @o_templateId = @productImageId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductImages_Filter]
    @productImageId INT = NULL,
    @productImageProductId INT = NULL,
    @productImageURL VARCHAR(200) = NULL,
    @productImageDescription VARCHAR(100) = NULL,
    @productImageIsPrincipal BIT = NULL,
    @productImageCreatorId INT = NULL,
    @productImageCreationDate DATETIME = NULL,
    @productImageModificatorId INT = NULL,
    @productImageModificationDate DATETIME = NULL,
    @productImageStatusId BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT productImageId, productImageProductId, productImageURL, productImageDescription, productImageIsPrincipal, productImageCreatorId, productImageCreationDate, productImageModificatorId, productImageModificationDate, productImageStatusId 
    FROM [SQM_GENERAL].[Tbl_ProductImages] (NOLOCK)
    WHERE (@productImageId IS NULL OR productImageId = @productImageId)
      AND (@productImageProductId IS NULL OR productImageProductId = @productImageProductId)
      AND (@productImageURL IS NULL OR productImageURL LIKE '%' + TRIM(@productImageURL) + '%')
      AND (@productImageDescription IS NULL OR productImageDescription LIKE '%' + TRIM(@productImageDescription) + '%')
      AND (@productImageIsPrincipal IS NULL OR productImageIsPrincipal = @productImageIsPrincipal)
      AND (@productImageCreatorId IS NULL OR productImageCreatorId = @productImageCreatorId)
      AND (@productImageCreationDate IS NULL OR CAST(productImageCreationDate AS DATE) = CAST(productImageCreationDate AS DATE))
      AND (@productImageModificatorId IS NULL OR productImageModificatorId = @productImageModificatorId)
      AND (@productImageModificationDate IS NULL OR CAST(productImageModificationDate AS DATE) = CAST(productImageModificationDate AS DATE))
      AND (@productImageStatusId IS NULL OR productImageStatusId = @productImageStatusId)
    OPTION (RECOMPILE);
END;
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_ProductImages_List]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT productImageId, productImageProductId, productImageURL, productImageDescription, productImageIsPrincipal, productImageCreatorId, productImageCreationDate, productImageModificatorId, productImageModificationDate, productImageStatusId 
    FROM [SQM_GENERAL].[Tbl_ProductImages] (NOLOCK);
END;
GO