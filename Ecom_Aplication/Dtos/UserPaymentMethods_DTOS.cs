using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class UserPaymentMethods_DTOS
    {
        public int? userPaymentMethodId { get; set; }
        public int? userPaymentMethodUserId { get; set; }
        public int? userPaymentMethodPaymentMethodTypeId { get; set; }
        public byte[]? userPaymentMethodCardNumber { get; set; }
        public byte[]? userPaymentMethodExpirationDate { get; set; }
        public byte[]? userPaymentMethodCVV { get; set; }
        public string? userPaymentMethodCardHolderName { get; set; }
        public int? userPaymentMethodCreatorId { get; set; }
        public DateTime? userPaymentMethodCreationDate { get; set; }
        public int? userPaymentMethodModificatorId { get; set; }
        public DateTime? userPaymentMethodModificationDate { get; set; }
        public bool? userPaymentMethodStatusId { get; set; }
    }
}