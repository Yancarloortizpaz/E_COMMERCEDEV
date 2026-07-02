using Ecom_Aplication.Interfaces;
using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class StockMovementTypes_Services : IStockMovementTypes
    {
        private readonly IStockMovementTypes _stockMovementTypesRepository;

        public StockMovementTypes_Services(IStockMovementTypes stockMovementTypesRepository)
        {
            _stockMovementTypesRepository = stockMovementTypesRepository;
        }

        public async Task<IEnumerable<StockMovementTypes>> FILTRAR_STOCKMOVEMENTTYPES_ASYNC(
            int? stockMovementTypeId,
            string stockMovementTypeName,
            string stockMovementTypeDescription,
            int? stockMovementTypeCreatorId,
            DateTime? stockMovementTypeCreationDate,
            int? stockMovementTypeModificatorId,
            DateTime? stockMovementTypeModificationDate,
            bool? stockMovementTypeStatusId)
        {
            return await _stockMovementTypesRepository.FILTRAR_STOCKMOVEMENTTYPES_ASYNC(
                stockMovementTypeId,
                stockMovementTypeName,
                stockMovementTypeDescription,
                stockMovementTypeCreatorId,
                stockMovementTypeCreationDate,
                stockMovementTypeModificatorId,
                stockMovementTypeModificationDate,
                stockMovementTypeStatusId
            );
        }
    }
}
