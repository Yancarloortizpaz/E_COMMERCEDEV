using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class Providers_DTOS
    {
        public int? ProviderId { get; set; }
        public string? ProviderName { get; set; }
        public string? ProviderDescription { get; set; }
        public int? ProviderCreatorId { get; set; }
        public DateTime? ProviderCreationDate { get; set; }
        public int? ProviderModificatorId { get; set; }
        public DateTime? ProviderModificationDate { get; set; }
        public bool? ProviderStatusId { get; set; }
    }
}