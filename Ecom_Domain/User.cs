using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Domain
{
    public class User
    {
        public int? UserId { get; set; }
        public string? UserFullName { get; set; }
        public string? UserName { get; set; }
        public byte[]? UserPassword { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhoneNumber { get; set; }
        public int? UserCountryId { get; set; }
        public int? UserGenderId { get; set; }
        public DateTime? UserBirthDay { get; set; }
        public int? UserCreatorId { get; set; }
        public DateTime? UserCreationDate { get; set; }
        public int? UserModificatorId { get; set; }
        public DateTime? UserModificationDate { get; set; }
        public int? UserStatusId { get; set; }
    }
}