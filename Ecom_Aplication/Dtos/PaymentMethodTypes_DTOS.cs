using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class PaymentMethodTypes_DTOS
    {
        public int? paymentMethodTypeId { get; set; }
        public string? paymentMethodTypeName { get; set; }
        public string? paymentMethodTypeDescription { get; set; }
        public int? paymentMethodTypeCreatorId { get; set; }
        public DateTime? paymentMethodTypeCreationDate { get; set; }
        public int? paymentMethodTypeModificatorId { get; set; }
        public DateTime? paymentMethodTypeModificationDate { get; set; }
        public bool? paymentMethodTypeStatusId { get; set; }
    }
}