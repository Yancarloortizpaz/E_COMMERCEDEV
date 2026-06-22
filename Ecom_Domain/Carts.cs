using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Domain
{
    public class Carts
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