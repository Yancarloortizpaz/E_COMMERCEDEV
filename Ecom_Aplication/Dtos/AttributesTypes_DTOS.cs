using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class AttributesTypes_DTOS
    {
        public int? AttributeTypeId { get; set; }
        public string? AttributeTypeName { get; set; }
        public string? AttributeTypeDescription { get; set; }
        public int? AttributeTypeCreatorId { get; set; }
        public DateTime? AttributeTypeCreationDate { get; set; }
        public int? AttributeTypeModificatorId { get; set; }
        public DateTime? AttributeTypeModificationDate { get; set; }
        public bool? AttributeTypeStatusId { get; set; }
    }
}