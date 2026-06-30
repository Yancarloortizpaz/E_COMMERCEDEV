using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class ProductVariables_DTOS
    {
        public int? productVariableId { get; set; }
        public int? productVariableProductId { get; set; }
        public string? productVariableValue { get; set; }
        public decimal? productVariablePrice { get; set; }
        public int? productVariableCurrencyId { get; set; }
        public int? productVariableCreatorId { get; set; }
        public DateTime? productVariableCreationDate { get; set; }
        public int? productVariableModificatorId { get; set; }
        public DateTime? productVariableModificationDate { get; set; }
        public bool? productVariableStatusId { get; set; }
    }
}
