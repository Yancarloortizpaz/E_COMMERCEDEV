using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class PaymentOrders_DTOs
    {
        public int? orderId { get; set; }
        public int? orderUserId { get; set; }
        public int? orderDeliveryAddress { get; set; }
        public int? orderPaymentMethodId { get; set; }
        public Decimal? orderSubtotal { get; set; }
        public Decimal? orderDiscount { get; set; }
        public Decimal? orderShipping { get; set; }
        public Decimal? orderTAX { get; set; }
        public Decimal? orderTotal { get; set; }
        public int? orderCurrencyId { get; set; }
        public int? orderCreatorId { get; set; }
        public DateTime? orderCreationDate { get; set; }
        public int? orderModificatorId { get; set; }
        public DateTime? orderModificationDate { get; set; }
        public int? orderStatusId { get; set; }

        
    }
}



   
   
    
    
    
    
    
    
    
    
    