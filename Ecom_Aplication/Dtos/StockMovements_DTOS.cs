using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class StockMovements_DTOS
    {
        public int? stockMovementId { get; set; }
        public int? stockMovementType { get; set; }
        public int? stockMovementOrderId { get; set; }
        public string? stockMovementReference { get; set; }
        public DateTime? stockMovementDate { get; set; }
        public int? stockMovementCreatorId { get; set; }
        public DateTime? stockMovementCreationDate { get; set; }
        public int? stockMovementModifierId { get; set; }
        public DateTime? stockMovementModificationDate { get; set; }
        public int? stockMovementStatusId { get; set; }
    }
}