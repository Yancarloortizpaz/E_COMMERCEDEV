USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Status_Create]
(
    @statusName VARCHAR(50),
    @statusCreatorId INT,
    @statusStatusId INT,
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
        INSERT INTO [SQM_CATALOGS].[Tbl_Status] 
            (statusName, statusCreatorId, statusCreationDate, statusStatusId)
        VALUES 
            (TRIM(@statusName), @statusCreatorId, GETDATE(), @statusStatusId);
        
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

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Status_Update]
(
    @statusId INT,
    @statusName VARCHAR(50),
    @statusCreatorId INT,
    @statusStatusId INT,
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
        UPDATE [SQM_CATALOGS].[Tbl_Status]
        SET statusName = TRIM(@statusName),
            statusCreatorId = @statusCreatorId,
            statusStatusId = @statusStatusId
        WHERE statusId = @statusId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado exitosamente.'; SET @o_templateId = @statusId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Status_Delete]
(
    @statusId INT,
    @statusStatusId INT,
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
        UPDATE [SQM_CATALOGS].[Tbl_Status] 
        SET statusStatusId = @statusStatusId 
        WHERE statusId = @statusId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado/deshabilitado.'; SET @o_templateId = @statusId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Status_Filter]
    @statusId INT = NULL,
    @statusName VARCHAR(50) = NULL,
    @statusCreatorId INT = NULL,
    @statusCreationDate DATETIME = NULL,
    @statusStatusId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT statusId, statusName, statusCreatorId, statusCreationDate, statusStatusId 
    FROM [SQM_CATALOGS].[Tbl_Status] (NOLOCK)
    WHERE (@statusId IS NULL OR statusId = @statusId)
      AND (@statusName IS NULL OR statusName LIKE '%' + TRIM(@statusName) + '%')
      AND (@statusCreatorId IS NULL OR statusCreatorId = @statusCreatorId)
      AND (@statusCreationDate IS NULL OR CAST(statusCreationDate AS DATE) = CAST(@statusCreationDate AS DATE))
      AND (@statusStatusId IS NULL OR statusStatusId = @statusStatusId)
    OPTION (RECOMPILE);
END;
GO

CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_Status_List]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT statusId, statusName, statusCreatorId, statusCreationDate, statusStatusId 
    FROM [SQM_CATALOGS].[Tbl_Status] (NOLOCK);
END;
GO