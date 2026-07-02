USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PriceHistory_Create]
(
    @priceHistoryProductVariableId INT,
    @priceHistoryOldPrice DECIMAL(18,2),
    @priceHistoryNewPrice DECIMAL(18,2),
    @priceHistoryModifierId INT = NULL,
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
        INSERT INTO [SQM_GENERAL].[Tbl_PriceHistory] 
            (priceHistoryProductVariableId, priceHistoryOldPrice, priceHistoryNewPrice, priceHistoryChangeDate, priceHistoryModifierId)
        VALUES 
            (@priceHistoryProductVariableId, @priceHistoryOldPrice, @priceHistoryNewPrice, GETDATE(), @priceHistoryModifierId);
        
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

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PriceHistory_Update]
(
    @priceHistoryId INT,
    @priceHistoryProductVariableId INT,
    @priceHistoryOldPrice DECIMAL(18,2),
    @priceHistoryNewPrice DECIMAL(18,2),
    @priceHistoryModifierId INT = NULL,
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
        UPDATE [SQM_GENERAL].[Tbl_PriceHistory]
        SET priceHistoryProductVariableId = @priceHistoryProductVariableId,
            priceHistoryOldPrice = @priceHistoryOldPrice,
            priceHistoryNewPrice = @priceHistoryNewPrice,
            priceHistoryChangeDate = GETDATE(),
            priceHistoryModifierId = @priceHistoryModifierId
        WHERE priceHistoryId = @priceHistoryId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Registro actualizado exitosamente.'; SET @o_templateId = @priceHistoryId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PriceHistory_Delete]
(
    @priceHistoryId INT,
    @priceHistoryProductVariableId INT,
    @priceHistoryOldPrice DECIMAL(18,2),
    @priceHistoryNewPrice DECIMAL(18,2),
    @priceHistoryModifierId INT = NULL,
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
        -- Modifica por completo todos los valores en el DDL simulando eliminación o forzando la actualización total de datos
        UPDATE [SQM_GENERAL].[Tbl_PriceHistory]
        SET priceHistoryProductVariableId = @priceHistoryProductVariableId,
            priceHistoryOldPrice = @priceHistoryOldPrice,
            priceHistoryNewPrice = @priceHistoryNewPrice,
            priceHistoryChangeDate = GETDATE(),
            priceHistoryModifierId = @priceHistoryModifierId
        WHERE priceHistoryId = @priceHistoryId;
        
        COMMIT TRANSACTION;
        SET @o_code = 200; SET @o_message = 'Campos del registro actualizados en borrado lógico.'; SET @o_templateId = @priceHistoryId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SET @o_code = ERROR_NUMBER(); SET @o_message = ERROR_MESSAGE();
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PriceHistory_Filter]
    @priceHistoryId INT = NULL,
    @priceHistoryProductVariableId INT = NULL,
    @priceHistoryOldPrice DECIMAL(18,2) = NULL,
    @priceHistoryNewPrice DECIMAL(18,2) = NULL,
    @priceHistoryChangeDate DATETIME = NULL,
    @priceHistoryModifierId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT priceHistoryId, priceHistoryProductVariableId, priceHistoryOldPrice, priceHistoryNewPrice, priceHistoryChangeDate, priceHistoryModifierId 
    FROM [SQM_GENERAL].[Tbl_PriceHistory] (NOLOCK)
    WHERE (@priceHistoryId IS NULL OR priceHistoryId = @priceHistoryId)
      AND (@priceHistoryProductVariableId IS NULL OR priceHistoryProductVariableId = @priceHistoryProductVariableId)
      AND (@priceHistoryOldPrice IS NULL OR priceHistoryOldPrice = @priceHistoryOldPrice)
      AND (@priceHistoryNewPrice IS NULL OR priceHistoryNewPrice = @priceHistoryNewPrice)
      AND (@priceHistoryChangeDate IS NULL OR CAST(priceHistoryChangeDate AS DATE) = CAST(priceHistoryChangeDate AS DATE))
      AND (@priceHistoryModifierId IS NULL OR priceHistoryModifierId = @priceHistoryModifierId)
    OPTION (RECOMPILE);
END;
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_PriceHistory_List]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT priceHistoryId, priceHistoryProductVariableId, priceHistoryOldPrice, priceHistoryNewPrice, priceHistoryChangeDate, priceHistoryModifierId 
    FROM [SQM_GENERAL].[Tbl_PriceHistory] (NOLOCK);
END;
GO