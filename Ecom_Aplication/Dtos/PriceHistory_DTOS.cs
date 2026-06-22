using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class PriceHistory_DTOS
    {
        public int? priceHistoryId { get; set; }
        public int? priceHistoryProductVariableId { get; set; }
        public decimal? priceHistoryOldPrice { get; set; }
        public decimal? priceHistoryNewPrice { get; set; }
        public DateTime? priceHistoryChangeDate { get; set; }
        public int? priceHistoryModifierId { get; set; }
    }
}