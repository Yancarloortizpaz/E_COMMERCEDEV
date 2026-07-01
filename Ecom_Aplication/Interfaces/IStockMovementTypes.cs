using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IStockMovementTypes
    {
        Task<IEnumerable<StockMovementTypes>> FILTRAR_STOCKMOVEMENTTYPES_ASYNC(
            int? stockMovementTypeId,
            string stockMovementTypeName,
            string stockMovementTypeDescription,
            int? stockMovementTypeCreatorId,
            DateTime? stockMovementTypeCreationDate,
            int? stockMovementTypeModificatorId,
            DateTime? stockMovementTypeModificationDate,
            bool? stockMovementTypeStatusId
        );
    }
}