using Ecom_Aplication.Interfaces;
using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class StockMovements_Services : IStockMovements
    {
        private readonly IStockMovements _stockMovementsRepository;

        public StockMovements_Services(IStockMovements stockMovementsRepository)
        {
            _stockMovementsRepository = stockMovementsRepository;
        }

        public async Task<IEnumerable<StockMovements>> LISTAR_STOCKMOVEMENTS_ASYNC()
        {
            return await _stockMovementsRepository.LISTAR_STOCKMOVEMENTS_ASYNC();
        }

        public async Task<IEnumerable<StockMovements>> FILTRAR_STOCKMOVEMENTS_ASYNC(string searchTerm)
        {
            return await _stockMovementsRepository.FILTRAR_STOCKMOVEMENTS_ASYNC(searchTerm);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_STOCKMOVEMENTS_ASYNC(
            int stockMovementType,
            int? stockMovementOrderId,
            string stockMovementReference,
            DateTime stockMovementDate,
            int stockMovementCreatorId,
            int stockMovementStatusId)
        {
            return await _stockMovementsRepository.NUEVO_STOCKMOVEMENTS_ASYNC(
                stockMovementType,
                stockMovementOrderId,
                stockMovementReference,
                stockMovementDate,
                stockMovementCreatorId,
                stockMovementStatusId
            );
        }
    }
}