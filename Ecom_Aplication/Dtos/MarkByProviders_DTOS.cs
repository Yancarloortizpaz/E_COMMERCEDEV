using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class MarkByProviders_DTOS
    {
        public int? MarkByProviderId { get; set; }
        public int? MarkByProviderMarkId { get; set; }
        public int? MarkByProviderProviderId { get; set; }
        public int? MarkByProviderCreatorId { get; set; }
        public DateTime? MarkByProviderCreationDate { get; set; }
        public int? MarkByProviderModificatorId { get; set; }
        public DateTime? MarkByProviderModificationDate { get; set; }
        public bool? MarkByProviderStatusId { get; set; }
    }
}