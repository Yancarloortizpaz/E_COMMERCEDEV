CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_Filter]
(
    @SearchTerm VARCHAR(50) = NULL,
    @i_pageNumber INT = 1,            
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_pageNumber INT = NULL OUTPUT,
    @o_pageSize INT = NULL OUTPUT,
    @o_totalRows INT = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
      
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



            CREATE OR ALTER PROCEDURE [SQM_GENERAL].[sp_Products_List]
(
    @i_pageNumber INT = NULL,
    @o_code INT = NULL OUTPUT,
    @o_message VARCHAR(255) = NULL OUTPUT,
    @o_pageNumber INT = NULL OUTPUT,
    @o_pageSize INT = NULL OUTPUT,
    @o_totalRows INT = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PageSize INT = 5;

    BEGIN TRY
      
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

            DONDE TENEMOS
             AQUI TENEMOS LOS ARHIVOS QUE NECESITAMOS MODIFICAR TENIENDO EN CUENTA QUE SOLO ES LISTAR Y FILTRAR 
            < C:\hector\E_COMMERCEDEV\API\API_ECCOMERCEDEV\APLICATION\DTOs\Products\



            C:\hector\E_COMMERCEDEV\API\API_ECCOMERCEDEV\APLICATION\Services\ProductsServices.cs
            C:\hector\E_COMMERCEDEV\API\API_ECCOMERCEDEV\DOMAIN\Products\
            C:\hector\E_COMMERCEDEV\API\API_ECCOMERCEDEV\INFRASTRUCTURE\Repository\ProductsRepository.cs