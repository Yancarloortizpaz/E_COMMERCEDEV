

USE [DB_EcommerceAgent]
GO

CREATE OR ALTER PROCEDURE dbo.SP_ListarGeneralProducts_Filtro
    @01_FilterText VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

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
        StockExpirationDate
    FROM DB_ECOMMERCE.SQM_GENERAL.VW_GENERAL_PRODUCTS
    WHERE 
        @01_FilterText IS NULL
        OR ProductName LIKE '%' + @01_FilterText + '%'
        OR ProductVariableName LIKE '%' + @01_FilterText + '%'
        OR CategoryName LIKE '%' + @01_FilterText + '%'
        OR SubcategoryName LIKE '%' + @01_FilterText + '%'
        OR SegmentName LIKE '%' + @01_FilterText + '%'
        OR MarkName LIKE '%' + @01_FilterText + '%'
        OR ProviderName LIKE '%' + @01_FilterText + '%'
        OR CurrencyISO LIKE '%' + @01_FilterText + '%';
END
GO



EXEC dbo.SP_ListarGeneralProducts_Filtro @01_FilterText = 'Dell';



EXEC dbo.SP_ListarGeneralProducts_Filtro @01_FilterText = 'NIKE';


