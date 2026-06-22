using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class Currencies_DTOS
    {
        public int? currencyId { get; set; }
        public string? currencyName { get; set; }
        public string? currencyISO { get; set; }
        public int? currencyCode { get; set; }
        public string? currencyDescription { get; set; }
        public int? currencyCreatorId { get; set; }
        public DateTime? currencyCreationDate { get; set; }
        public int? currencyModificatorId { get; set; }
        public DateTime? currencyModificationDate { get; set; }
        public bool? currencyStatusId { get; set; }
    }
}
