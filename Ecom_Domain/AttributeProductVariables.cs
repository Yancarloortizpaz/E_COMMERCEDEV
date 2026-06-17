using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Domain
{
    public class AttributeProductVariables
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
