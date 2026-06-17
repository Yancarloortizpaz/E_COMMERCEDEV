using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom_Aplication.Dtos
{
    public class Stocks_DTOS
    {
        public int? stockId { get; set; }
        public int stockProductVariableId { get; set; }
        public int stockQuantity { get; set; }
        public DateTime? stockFactoryDate { get; set; }
        public DateTime? stockExpirationDate { get; set; }
        public int stockCreatorId { get; set; }
        public DateTime? stockCreationDate { get; set; }
        public int stockModificatorId { get; set; }
        public DateTime? stockModificationDate { get; set; }
        public bool? stockStatusId { get; set; }
    }
}
