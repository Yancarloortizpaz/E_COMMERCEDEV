using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class PaymentOrderDetails_DTOs
    {
        public int? orderDetailId { get; set; }
        public int? orderDetailOrderId { get; set; }
        public int? orderDetailProductVariableId { get; set; }
        public decimal? orderDetailPrice { get; set; }
        public int? orderDetailQuantity { get; set; }
        public decimal? orderDetailDiscount { get; set; }
        public decimal? orderDetailSubTotal { get; set; }
        public decimal? orderDetailTAX { get; set; }
        public decimal? orderDetailTotal { get; set; }
        public int? orderDetailCurrencyId { get; set; }
        public int? orderDetailCreatorId { get; set; }
        public DateTime? orderDetailCreationDate { get; set; }
        public int? orderDetailModificatorId { get; set; }
        public DateTime? orderDetailModificationDate { get; set; }
        public DateTime? orderDetailStatusId { get; set; }
    }
}
