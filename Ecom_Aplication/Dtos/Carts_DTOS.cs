using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class Carts_DTOS
    {
        public int? cartId { get; set; }
        public int? cartUserId { get; set; }
        public int? cartCreatorId { get; set; }
        public DateTime? cartCreationDate { get; set; }
        public int? cartModificatorId { get; set; }
        public DateTime? cartModificationDate { get; set; }
        public bool? cartStatusId { get; set; }
    }
}
