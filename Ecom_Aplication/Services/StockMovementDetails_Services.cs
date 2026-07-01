using Ecom_Aplication.Interfaces;
using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class StockMovementDetails_Services : IStockMovementDetails
    {
        private readonly IStockMovementDetails _stockMovementDetailsRepository;

        public StockMovementDetails_Services(IStockMovementDetails stockMovementDetailsRepository)
        {
            _stockMovementDetailsRepository = stockMovementDetailsRepository;
        }

        public async Task<IEnumerable<StockMovementDetails>> LISTAR_STOCKMOVEMENTDETAILS_ASYNC()
        {
            return await _stockMovementDetailsRepository.LISTAR_STOCKMOVEMENTDETAILS_ASYNC();
        }

        public async Task<IEnumerable<StockMovementDetails>> FILTRAR_STOCKMOVEMENTDETAILS_ASYNC(int? movementId, string searchTerm)
        {
            return await _stockMovementDetailsRepository.FILTRAR_STOCKMOVEMENTDETAILS_ASYNC(movementId, searchTerm);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_STOCKMOVEMENTDETAILS_ASYNC(
            int stockMovementDetailMovementId,
            int? stockMovementDetailOrderDetailId,
            int? stockMovementDetailStockId,
            int stockMovementDetailQuantity,
            DateTime? stockMovementDetailFactoryDate,
            DateTime? stockMovementDetailExpirationDate,
            int stockMovementDetailCreatorId,
            bool stockMovementDetailStatusId)
        {
            return await _stockMovementDetailsRepository.NUEVO_STOCKMOVEMENTDETAILS_ASYNC(
                stockMovementDetailMovementId,
                stockMovementDetailOrderDetailId,
                stockMovementDetailStockId,
                stockMovementDetailQuantity,
                stockMovementDetailFactoryDate,
                stockMovementDetailExpirationDate,
                stockMovementDetailCreatorId,
                stockMovementDetailStatusId
            );
        }
    }
}