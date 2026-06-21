using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class UserPaymentMethods_DTOs
    {
        public int? userPaymentMethodId { get; set; }
        public int? userPaymentMethodUserId { get; set; }
        public int? userPaymentMethodPaymentMethodTypeId { get; set; }
        public string? userPaymentMethodCardNumber { get; set; }
        public string? userPaymentMethodExpirationDate { get; set; }
        public string? userPaymentMethodCVV { get; set; }
        public string? userPaymentMethodCardHolderName { get; set; }
        public int? userPaymentMethodCreatorId { get; set; }
        public DateTime? userPaymentMethodCreationDate { get; set; }
        public int? userPaymentMethodModificatorId { get; set; }
        public DateTime? userPaymentMethodModificationDate { get; set; }
        public bool? userPaymentMethodStatusId { get; set; }
        
    }
}


    
    
    
    
   
 
    
    
   
    