using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Domain
{
    public class StockMovementDetails
    {
        public int? stockMovementDetailId { get; set; }
        public int? stockMovementDetailMovementId { get; set; }
        public int? stockMovementDetailOrderDetailId { get; set; }
        public int? stockMovementDetailStockId { get; set; }
        public int? stockMovementDetailQuantity { get; set; }
        public DateTime? stockMovementDetailFactoryDate { get; set; }
        public DateTime? stockMovementDetailExpirationDate { get; set; }
        public int? stockMovementDetailCreatorId { get; set; }
        public DateTime? stockMovementDetailCreationDate { get; set; }
        public int? stockMovementDetailModifierId { get; set; }
        public DateTime? stockMovementDetailModificationDate { get; set; }
        public bool? stockMovementDetailStatusId { get; set; }
    }
}