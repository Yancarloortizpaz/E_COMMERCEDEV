CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_MarkByProviders_Create]
(
    @markByProviderMarkId INT,
    @markByProviderProviderId INT,
    @markByProviderCreatorId INT,
    @markByProviderModificatorId INT = NULL,
    @markByProviderStatusId BIT,
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
        INSERT INTO [SQM_CATALOGS].[Tbl_MarkByProviders] 
            (markByProviderMarkId, markByProviderProviderId, markByProviderCreatorId, markByProviderCreationDate, markByProviderModificatorId, markByProviderModificationDate, markByProviderStatusId)
        VALUES 
            (@markByProviderMarkId, @markByProviderProviderId, @markByProviderCreatorId, GETDATE(), @markByProviderModificatorId, NULL, @markByProviderStatusId);
        
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

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_MarkByProviders_Update]
(
    @markByProviderId INT,
    @markByProviderMarkId INT,
    @markByProviderProviderId INT,
    @markByProviderCreatorId INT,
    @markByProviderModificatorId INT,
    @markByProviderStatusId BIT,
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
        UPDATE [SQM_CATALOGS].[Tbl_MarkByProviders]
        SET markByProviderMarkId = @markByProviderMarkId,
            markByProviderProviderId = @markByProviderProviderId,
            markByProviderCreatorId = @markByProviderCreatorId,
            markByProviderModificatorId = @markByProviderModificatorId,
            markByProviderModificationDate = GETDATE(),
            markByProviderStatusId = @markByProviderStatusId
        WHERE markByProviderId = @markByProviderId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado exitosamente.'; SET @o_templateId = @markByProviderId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_MarkByProviders_Delete]
(
    @markByProviderId INT,
    @markByProviderMarkId INT,
    @markByProviderProviderId INT,
    @markByProviderCreatorId INT,
    @markByProviderModificatorId INT,
    @markByProviderStatusId BIT,
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
        UPDATE [SQM_CATALOGS].[Tbl_MarkByProviders] 
        SET markByProviderMarkId = @markByProviderMarkId,
            markByProviderProviderId = @markByProviderProviderId,
            markByProviderCreatorId = @markByProviderCreatorId,
            markByProviderModificatorId = @markByProviderModificatorId, 
            markByProviderModificationDate = GETDATE(),
            markByProviderStatusId = @markByProviderStatusId
        WHERE markByProviderId = @markByProviderId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado/deshabilitado.'; SET @o_templateId = @markByProviderId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_MarkByProviders_Filter]
    @markByProviderId INT = NULL,
    @markByProviderMarkId INT = NULL,
    @markByProviderProviderId INT = NULL,
    @markByProviderCreatorId INT = NULL,
    @markByProviderCreationDate DATETIME = NULL,
    @markByProviderModificatorId INT = NULL,
    @markByProviderModificationDate DATETIME = NULL,
    @markByProviderStatusId BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT markByProviderId, markByProviderMarkId, markByProviderProviderId, markByProviderCreatorId, markByProviderCreationDate, markByProviderModificatorId, markByProviderModificationDate, markByProviderStatusId 
    FROM [SQM_CATALOGS].[Tbl_MarkByProviders] (NOLOCK)
    WHERE (@markByProviderId IS NULL OR markByProviderId = @markByProviderId)
      AND (@markByProviderMarkId IS NULL OR markByProviderMarkId = @markByProviderMarkId)
      AND (@markByProviderProviderId IS NULL OR markByProviderProviderId = @markByProviderProviderId)
      AND (@markByProviderCreatorId IS NULL OR markByProviderCreatorId = @markByProviderCreatorId)
      AND (@markByProviderCreationDate IS NULL OR CAST(markByProviderCreationDate AS DATE) = CAST(@markByProviderCreationDate AS DATE))
      AND (@markByProviderModificatorId IS NULL OR markByProviderModificatorId = @markByProviderModificatorId)
      AND (@markByProviderModificationDate IS NULL OR CAST(markByProviderModificationDate AS DATE) = CAST(@markByProviderModificationDate AS DATE))
      AND (@markByProviderStatusId IS NULL OR markByProviderStatusId = @markByProviderStatusId)
    OPTION (RECOMPILE);
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_MarkByProviders_List]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT markByProviderId, markByProviderMarkId, markByProviderProviderId, markByProviderCreatorId, markByProviderCreationDate, markByProviderModificatorId, markByProviderModificationDate, markByProviderStatusId 
    FROM [SQM_CATALOGS].[Tbl_MarkByProviders] (NOLOCK);
END;
GO