namespace DOMAIN.Products
{
    public class DM_Products_listar
    {
        public int? productId { get; set; }
        public string? productName { get; set; }
        public string? productDescription { get; set; }
        public int? productIdentificatorId { get; set; }
        public int? categoryId { get; set; }
        public string? categoryName { get; set; }
        public int? subCategoryId { get; set; }
        public string? subCategoryName { get; set; }
        public int? segmentId { get; set; }
        public string? segmentName { get; set; }
        public int? markByProviderId { get; set; }
        public int? markId { get; set; }
        public string? markName { get; set; }
        public int? providerId { get; set; }
        public string? providerName { get; set; }
        public bool? statusId { get; set; }
    }
}
