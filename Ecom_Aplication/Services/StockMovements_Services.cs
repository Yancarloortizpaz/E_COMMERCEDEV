using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class StockMovements_Services : IStockMovements
    {
        private readonly IStockMovements _repository;

        public StockMovements_Services(IStockMovements repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<StockMovements>> LISTAR_STOCKMOVEMENTS_ASYNC()
        {
            return await _repository.LISTAR_STOCKMOVEMENTS_ASYNC();
        }

        public async Task<IEnumerable<StockMovements>> FILTRAR_STOCKMOVEMENTS_ASYNC(string searchTerm)
        {
            return await _repository.FILTRAR_STOCKMOVEMENTS_ASYNC(searchTerm);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_STOCKMOVEMENTS_ASYNC(
            int stockMovementType,
            int? stockMovementOrderId,
            string stockMovementReference,
            DateTime stockMovementDate,
            int stockMovementCreatorId,
            int stockMovementStatusId)
        {
            return await _repository.NUEVO_STOCKMOVEMENTS_ASYNC(
                stockMovementType,
                stockMovementOrderId,
                stockMovementReference,
                stockMovementDate,
                stockMovementCreatorId,
                stockMovementStatusId
            );
        }

 
        public async Task<(int code, string message, int? templateId)> NUEVO_STOCKMOVEMENTS_ASYNC(StockMovements_DTOS dto)
        {
          
            return await _repository.NUEVO_STOCKMOVEMENTS_ASYNC(
                dto.stockMovementType ?? 0,
                dto.stockMovementOrderId,
                dto.stockMovementReference ?? "",
                dto.stockMovementDate ?? DateTime.Now,
                dto.stockMovementCreatorId ?? 0,
                dto.stockMovementStatusId ?? 0
            );
        }
    }
}