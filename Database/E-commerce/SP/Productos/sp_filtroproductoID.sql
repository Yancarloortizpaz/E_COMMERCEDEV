USE [DB_ECOMMERCE]
GO

CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_Filter_Id]
(
    @ProductId INT = NULL,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validar si el producto existe
        IF EXISTS (
            SELECT 1 
            FROM [SQM_GENERAL].[VW_GENERAL_PRODUCTS] (NOLOCK) 
            WHERE ProductID = @ProductId
        )
        BEGIN
            SELECT
                ProductID,
                ProductName,
                ProductVariableID,
                ProductVariableName,
                ProductVariablePrice,
                CurrencyID,
                CurrencyISO,
                CategoryID,
                CategoryName,
                SubcategoryID,
                SubcategoryName,
                SegmentID,
                SegmentName,
                MarkID,
                MarkName,
                ProviderID,
                ProviderName,
                StockID,
                StockAvilable,
                StockFactoryDate,
                StockExpirationDate,
                ProductImageURL
            FROM [SQM_GENERAL].[VW_GENERAL_PRODUCTS] (NOLOCK)
            WHERE ProductID = @ProductId;

            SET @o_code = 200;
            SET @o_message = 'Búsqueda de producto satisfactoria';
        END
        ELSE
        BEGIN
            SET @o_code = 204;
            SET @o_message = 'No se encontró el producto especificado';
        END

    END TRY
    BEGIN CATCH
        SET @o_code = 500;
        SET @o_message = CONCAT_WS(' ', 'Error interno:', ERROR_MESSAGE());
    END CATCH
END
GO








-- Caso 2: Filtrar solo por ID de producto (El SP convertirá internamente el texto a INT)
EXEC [SQM_GENERAL].[sp_Products_Filter_Id] 
    @ProductId = '2';
GO


-- Caso 3: Filtrar solo por coincidencia de texto (Aplica para Nombre, Categoría, Marca, Proveedor, etc.)
EXEC [SQM_GENERAL].[sp_Products_Filter_Id] '18'

GO


	
SELECT 
    COLUMN_NAME AS Campo,
    DATA_TYPE AS TipoDato,
    CHARACTER_MAXIMUM_LENGTH AS LongitudMaxima,
    IS_NULLABLE AS PermiteNull
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'SQM_GENERAL'
  AND TABLE_NAME = 'VW_GENERAL_PRODUCTS'
ORDER BY ORDINAL_POSITION;


