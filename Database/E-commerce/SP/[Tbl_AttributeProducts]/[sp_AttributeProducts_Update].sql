CREATE OR ALTER PROCEDURE [SQM_CATALOGS].[sp_AttributeProducts_Update]
(
    @AttributeProductId INT,
    @AttributeProductAttributesTypeId INT,
    @AttributeProductName VARCHAR(50),
    @AttributeProductDescription VARCHAR(100),
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

        IF NOT EXISTS
        (
            SELECT 1
            FROM [SQM_CATALOGS].[Tbl_AttributeProducts]
            WHERE AttributeProductId = @AttributeProductId
        )
        BEGIN
            SET @o_code = 404;
            SET @o_message = 'Registro no encontrado.';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        UPDATE [SQM_CATALOGS].[Tbl_AttributeProducts]
        SET
            AttributeProductAttributesTypeId = @AttributeProductAttributesTypeId,
            AttributeProductName = LTRIM(RTRIM(@AttributeProductName)),
            AttributeProductDescription = LTRIM(RTRIM(@AttributeProductDescription)),
            AttributeProductModificatorId = @AttributeProductModificatorId,
            AttributeProductModificationDate = GETDATE(),
            AttributeProductStatusId = @AttributeProductStatusId
        WHERE AttributeProductId = @AttributeProductId;

        COMMIT TRANSACTION;

        SET @o_code = 200;
        SET @o_message = 'Registro actualizado exitosamente.';
        SET @o_templateId = @AttributeProductId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SET @o_code = ERROR_NUMBER();
        SET @o_message = ERROR_MESSAGE();
    END CATCH
END;
GO