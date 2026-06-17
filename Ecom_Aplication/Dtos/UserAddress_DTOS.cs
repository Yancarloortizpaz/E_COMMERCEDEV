using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Domain
{
    public class UserAddress_DTOS
    {
        public int? UserAddressId { get; set; }
        public int? UserAddressUserId { get; set; }
        public int? UserAddressCountryId { get; set; }
        public int? UserAddressZIPCode { get; set; }
        public string? UserAddressDescription { get; set; }
        public bool? UserAddressIsPrincipal { get; set; }
        public int? UserAddressCreatorId { get; set; }
        public DateTime? UserAddressCreationDate { get; set; }
        public int? UserAddressModificatorId { get; set; }
        public DateTime? UserAddressModificationDate { get; set; }
        public bool? UserAddressStatusId { get; set; }
    }
}