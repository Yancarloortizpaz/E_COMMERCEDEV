using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class Products_DTOS
    {
        public int? productId { get; set; }
        public string? productName { get; set; }
        public string? productDescription { get; set; }
        public int? productProductIdentificatorId { get; set; }
        public int? productMarkByProviderId { get; set; }
        public int? productCreatorId { get; set; }
        public DateTime? productCreationDate { get; set; }
        public int? productModificatorId { get; set; }
        public DateTime? productModificationDate { get; set; }
        public bool? productStatusId { get; set; }
    }
}
