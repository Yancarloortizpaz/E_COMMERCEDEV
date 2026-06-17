using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class AttributeProductVariables_DTOS
    {
        public int? attributeProductVariableId { get; set; }
        public int attributeProductVariableProductVariableId { get; set; }
        public int attributeProductVariableAttributeProductId { get; set; }
        public string? attributeProductVariableValue { get; set; }
        public int attributeProductVariableCreatorId { get; set; }
        public DateTime? attributeProductVariableCreationDate { get; set; }
        public int attributeProductVariableModificatorId { get; set; }
        public DateTime? attributeProductVariableModificationDate { get; set; }
        public bool? attributeProductVariableStatusId { get; set; }
    }
}
