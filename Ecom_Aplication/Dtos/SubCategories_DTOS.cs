using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Domain
{
    public class SubCategories_DTOS
    {
        public int? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public string? SubCategoryDescription { get; set; }
        public int? SubCategoryCreatorId { get; set; }
        public DateTime? SubCategoryCreationDate { get; set; }
        public int? SubCategoryModificatorId { get; set; }
        public DateTime? SubCategoryModificationDate { get; set; }
        public bool? SubCategoryStatusId { get; set; }
    }
}