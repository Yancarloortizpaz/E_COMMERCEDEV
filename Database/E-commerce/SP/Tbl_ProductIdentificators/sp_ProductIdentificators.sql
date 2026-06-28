USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductIdentificators_Create]
(
    @productIdentificatorCategoryId INT,
    @productIdentificatorSubCategoryId INT,
    @productIdentificatorSegmentId INT,
    @productIdentificatorCreatorId INT,
    @productIdentificatorModificatorId INT = NULL,
    @productIdentificatorStatusId BIT,
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
        INSERT INTO [SQM_CATALOGS].[Tbl_ProductIdentificators] 
            (productIdentificatorCategoryId, productIdentificatorSubCategoryId, productIdentificatorSegmentId, productIdentificatorCreatorId, productIdentificatorCreationDate, productIdentificatorModificatorId, productIdentificatorModificationDate, productIdentificatorStatusId)
        VALUES 
            (@productIdentificatorCategoryId, @productIdentificatorSubCategoryId, @productIdentificatorSegmentId, @productIdentificatorCreatorId, GETDATE(), @productIdentificatorModificatorId, NULL, @productIdentificatorStatusId);
        
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

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductIdentificators_Update]
(
    @productIdentificatorId INT,
    @productIdentificatorCategoryId INT,
    @productIdentificatorSubCategoryId INT,
    @productIdentificatorSegmentId INT,
    @productIdentificatorCreatorId INT,
    @productIdentificatorModificatorId INT,
    @productIdentificatorStatusId BIT,
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
        UPDATE [SQM_CATALOGS].[Tbl_ProductIdentificators]
        SET productIdentificatorCategoryId = @productIdentificatorCategoryId,
            productIdentificatorSubCategoryId = @productIdentificatorSubCategoryId,
            productIdentificatorSegmentId = @productIdentificatorSegmentId,
            productIdentificatorCreatorId = @productIdentificatorCreatorId,
            productIdentificatorModificatorId = @productIdentificatorModificatorId,
            productIdentificatorModificationDate = GETDATE(),
            productIdentificatorStatusId = @productIdentificatorStatusId
        WHERE productIdentificatorId = @productIdentificatorId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado exitosamente.'; SET @o_templateId = @productIdentificatorId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductIdentificators_Delete]
(
    @productIdentificatorId INT,
    @productIdentificatorCategoryId INT,
    @productIdentificatorSubCategoryId INT,
    @productIdentificatorSegmentId INT,
    @productIdentificatorCreatorId INT,
    @productIdentificatorModificatorId INT,
    @productIdentificatorStatusId BIT,
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
        UPDATE [SQM_CATALOGS].[Tbl_ProductIdentificators] 
        SET productIdentificatorCategoryId = @productIdentificatorCategoryId,
            productIdentificatorSubCategoryId = @productIdentificatorSubCategoryId,
            productIdentificatorSegmentId = @productIdentificatorSegmentId,
            productIdentificatorCreatorId = @productIdentificatorCreatorId,
            productIdentificatorModificatorId = @productIdentificatorModificatorId, 
            productIdentificatorModificationDate = GETDATE(),
            productIdentificatorStatusId = @productIdentificatorStatusId
        WHERE productIdentificatorId = @productIdentificatorId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado/deshabilitado.'; SET @o_templateId = @productIdentificatorId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductIdentificators_Filter]
    @productIdentificatorId INT = NULL,
    @productIdentificatorCategoryId INT = NULL,
    @productIdentificatorSubCategoryId INT = NULL,
    @productIdentificatorSegmentId INT = NULL,
    @productIdentificatorCreatorId INT = NULL,
    @productIdentificatorCreationDate DATETIME = NULL,
    @productIdentificatorModificatorId INT = NULL,
    @productIdentificatorModificationDate DATETIME = NULL,
    @productIdentificatorStatusId BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT productIdentificatorId, productIdentificatorCategoryId, productIdentificatorSubCategoryId, productIdentificatorSegmentId, productIdentificatorCreatorId, productIdentificatorCreationDate, productIdentificatorModificatorId, productIdentificatorModificationDate, productIdentificatorStatusId 
    FROM [SQM_CATALOGS].[Tbl_ProductIdentificators] (NOLOCK)
    WHERE (@productIdentificatorId IS NULL OR productIdentificatorId = @productIdentificatorId)
      AND (@productIdentificatorCategoryId IS NULL OR productIdentificatorCategoryId = @productIdentificatorCategoryId)
      AND (@productIdentificatorSubCategoryId IS NULL OR productIdentificatorSubCategoryId = @productIdentificatorSubCategoryId)
      AND (@productIdentificatorSegmentId IS NULL OR productIdentificatorSegmentId = @productIdentificatorSegmentId)
      AND (@productIdentificatorCreatorId IS NULL OR productIdentificatorCreatorId = @productIdentificatorCreatorId)
      AND (@productIdentificatorCreationDate IS NULL OR CAST(productIdentificatorCreationDate AS DATE) = CAST(@productIdentificatorCreationDate AS DATE))
      AND (@productIdentificatorModificatorId IS NULL OR productIdentificatorModificatorId = @productIdentificatorModificatorId)
      AND (@productIdentificatorModificationDate IS NULL OR CAST(productIdentificatorModificationDate AS DATE) = CAST(@productIdentificatorModificationDate AS DATE))
      AND (@productIdentificatorStatusId IS NULL OR productIdentificatorStatusId = @productIdentificatorStatusId)
    OPTION (RECOMPILE);
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_ProductIdentificators_List]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT productIdentificatorId, productIdentificatorCategoryId, productIdentificatorSubCategoryId, productIdentificatorSegmentId, productIdentificatorCreatorId, productIdentificatorCreationDate, productIdentificatorModificatorId, productIdentificatorModificationDate, productIdentificatorStatusId 
    FROM [SQM_CATALOGS].[Tbl_ProductIdentificators] (NOLOCK);
END;
GO