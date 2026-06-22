using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class PaymentOrders_DTOS
    {
        public int? orderId { get; set; }
        public int? orderUserId { get; set; }
        public int? orderDeliveryAddress { get; set; }
        public int? orderPaymentMethodId { get; set; }
        public decimal? orderSubtotal { get; set; }
        public decimal? orderDiscount { get; set; }
        public decimal? orderShipping { get; set; }
        public decimal? orderTAX { get; set; }
        public decimal? orderTotal { get; set; }
        public int? orderCurrencyId { get; set; }
        public int? orderCreatorId { get; set; }
        public DateTime? orderCreationDate { get; set; }
        public int? orderModificatorId { get; set; }
        public DateTime? orderModificationDate { get; set; }
        public int? orderStatusId { get; set; }
    }
}