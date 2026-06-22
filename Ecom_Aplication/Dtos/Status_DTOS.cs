using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class Status_DTOS
    {
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public int? StatusCreatorId { get; set; }
        public DateTime? StatusCreationDate { get; set; }
        public int? StatusStatusId { get; set; }
    }
}