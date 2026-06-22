using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class StockMovementTypes_DTOS
    {
        public int? stockMovementTypeId { get; set; }
        public string? stockMovementTypeName { get; set; }
        public string? stockMovementTypeDescription { get; set; }
        public int? stockMovementTypeCreatorId { get; set; }
        public DateTime? stockMovementTypeCreationDate { get; set; }
        public int? stockMovementTypeModificatorId { get; set; }
        public DateTime? stockMovementTypeModificationDate { get; set; }
        public bool? stockMovementTypeStatusId { get; set; }
    }
}