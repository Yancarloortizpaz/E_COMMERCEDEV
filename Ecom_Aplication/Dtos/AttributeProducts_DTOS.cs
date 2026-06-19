using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class AttributeProducts_DTOS
    {
        public int? AttributeProductId { get; set; }
        public int AttributeProductAttributesTypeId { get; set; }
        public string? AttributeProductName { get; set; }
        public string? AttributeProductDescription { get; set; }
        public int AttributeProductCreatorId { get; set; }
        public DateTime? AttributeProductCreationDate { get; set; }
        public int AttributeProductModificatorId { get; set; }
        public DateTime? AttributeProductModificationDate { get; set; }
        public bool? AttributeProductStatusId { get; set; }
    }
}
