using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class CartDetail_DTO
    {
        public int? Id { get; set; }
        public int? CartId { get; set; }
        public int? ProductVariableId { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public decimal? Discount { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Total { get; set; }
        public int? CreatorId { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModificatorId { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool? StatusId { get; set; }
    }
}