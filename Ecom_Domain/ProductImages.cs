using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Domain
{
    public class ProductImages
    {
        public int? productImageId { get; set; }
        public int? productImageProductId { get; set; }
        public string? productImageURL { get; set; }
        public string? productImageDescription { get; set; }
        public bool? productImageIsPrincipal { get; set; }
        public int? productImageCreatorId { get; set; }
        public DateTime? productImageCreationDate { get; set; }
        public int? productImageModificatorId { get; set; }
        public DateTime? productImageModificationDate { get; set; }
        public bool? productImageStatusId { get; set; }
    }
}