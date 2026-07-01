using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IStockMovements
    {
        Task<IEnumerable<StockMovements>> LISTAR_STOCKMOVEMENTS_ASYNC();

        
        Task<IEnumerable<StockMovements>> FILTRAR_STOCKMOVEMENTS_ASYNC(string searchTerm);

        Task<(int code, string message, int? templateId)> NUEVO_STOCKMOVEMENTS_ASYNC(
            int stockMovementType,
            int? stockMovementOrderId,
            string stockMovementReference,
            DateTime stockMovementDate,
            int stockMovementCreatorId,
            int stockMovementStatusId
        );
    }
}