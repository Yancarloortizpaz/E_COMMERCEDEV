using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IStockMovementDetails
    {
       
        Task<IEnumerable<StockMovementDetails>> LISTAR_STOCKMOVEMENTDETAILS_ASYNC();

        
        Task<IEnumerable<StockMovementDetails>> FILTRAR_STOCKMOVEMENTDETAILS_ASYNC(int? movementId, string searchTerm);

        
        Task<(int code, string message, int? templateId)> NUEVO_STOCKMOVEMENTDETAILS_ASYNC(
            int stockMovementDetailMovementId,
            int? stockMovementDetailOrderDetailId,
            int? stockMovementDetailStockId,
            int stockMovementDetailQuantity,
            DateTime? stockMovementDetailFactoryDate,
            DateTime? stockMovementDetailExpirationDate,
            int stockMovementDetailCreatorId,
            bool stockMovementDetailStatusId
        );
    }
}