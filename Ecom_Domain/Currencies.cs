using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace Ecom_Domain
{
    public class Currencies
    {
        public int? currencyId { get; set; }
        public string? currencyName { get; set; }
        public string? currencyISO { get; set; }
        public int currencyCode { get; set; }
        public string? currencyDescription { get; set; }
        public int currencyCreatorId { get; set; }
        public DateTime? currencyCreationDate { get; set; }
        public int currencyModificatorId { get; set; }
        public DateTime? currencyModificationDate { get; set; }
        public bool? currencyStatusId { get; set; }
    }
}