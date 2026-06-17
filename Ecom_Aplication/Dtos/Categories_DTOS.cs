using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Domain
{
    public class Categories_DTOS
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public int? CategoryCreatorId { get; set; }
        public DateTime? CategoryCreationDate { get; set; }
        public int? CategoryModificatorId { get; set; }
        public DateTime? CategoryModificationDate { get; set; }
        public bool? CategoryStatusId { get; set; }
    }
}