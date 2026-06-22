using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class ProductIdentificators_DTOS
    {
        public int? ProductIdentificatorId { get; set; }
        public int? ProductIdentificatorCategoryId { get; set; }
        public int? ProductIdentificatorSubCategoryId { get; set; }
        public int? ProductIdentificatorSegmentId { get; set; }
        public int? ProductIdentificatorCreatorId { get; set; }
        public DateTime? ProductIdentificatorCreationDate { get; set; }
        public int? ProductIdentificatorModificatorId { get; set; }
        public DateTime? ProductIdentificatorModificationDate { get; set; }
        public bool? ProductIdentificatorStatusId { get; set; }
    }
}