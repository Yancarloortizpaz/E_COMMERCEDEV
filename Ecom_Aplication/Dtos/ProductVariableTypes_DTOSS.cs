using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class ProductVariableTypes_DTOSS
    {
        public int? productVariableTypeId { get; set; }
        public string? productVariableTypeName { get; set; }
        public string? productVariableTypeDescription { get; set; }
        public int productVariableTypeCreatorId { get; set; }
        public DateTime? productVariableTypeCreationDate { get; set; }
        public int productVariableTypeModificatorId { get; set; }
        public DateTime? productVariableTypeModificationDate { get; set; }
        public bool? productVariableTypeStatusId { get; set; }
    }
}
