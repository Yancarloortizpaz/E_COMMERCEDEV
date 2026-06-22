using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Domain
{
    public class PriceHistory
    {
        public int? priceHistoryId { get; set; }
        public int? priceHistoryProductVariableId { get; set; }
        public decimal? priceHistoryOldPrice { get; set; }
        public decimal? priceHistoryNewPrice { get; set; }
        public DateTime? priceHistoryChangeDate { get; set; }
        public int? priceHistoryModifierId { get; set; }
    }
}