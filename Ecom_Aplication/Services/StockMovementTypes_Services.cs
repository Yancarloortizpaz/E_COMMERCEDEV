using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class StockMovementTypes_Services
    {
        private readonly IStockMovementTypes _repository;

        public StockMovementTypes_Services(IStockMovementTypes repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<StockMovementTypes_DTOS>> LISTAR_STOCKMOVEMENTTYPES_ASYNC()
        {

            var data = await _repository.FILTRAR_STOCKMOVEMENTTYPES_ASYNC(null, "", "", null, null, null, null, null);

            return data.Select(s => new StockMovementTypes_DTOS
            {
                stockMovementTypeId = s.stockMovementTypeId,
                stockMovementTypeName = s.stockMovementTypeName,
                stockMovementTypeDescription = s.stockMovementTypeDescription,
                stockMovementTypeCreatorId = s.stockMovementTypeCreatorId,
                stockMovementTypeCreationDate = s.stockMovementTypeCreationDate,
                stockMovementTypeModificatorId = s.stockMovementTypeModificatorId,
                stockMovementTypeModificationDate = s.stockMovementTypeModificationDate,
                stockMovementTypeStatusId = s.stockMovementTypeStatusId
            });
        }

        public async Task<StockMovementTypes_DTOS?> OBTENER_STOCKMOVEMENTTYPE_BY_ID_ASYNC(int id)
        {
            // Filtramos específicamente por el ID del tipo de movimiento
            var data = await _repository.FILTRAR_STOCKMOVEMENTTYPES_ASYNC(id, "", "", null, null, null, null, null);

            return data.Select(s => new StockMovementTypes_DTOS
            {
                stockMovementTypeId = s.stockMovementTypeId,
                stockMovementTypeName = s.stockMovementTypeName,
                stockMovementTypeDescription = s.stockMovementTypeDescription,
                stockMovementTypeCreatorId = s.stockMovementTypeCreatorId,
                stockMovementTypeCreationDate = s.stockMovementTypeCreationDate,
                stockMovementTypeModificatorId = s.stockMovementTypeModificatorId,
                stockMovementTypeModificationDate = s.stockMovementTypeModificationDate,
                stockMovementTypeStatusId = s.stockMovementTypeStatusId
            }).FirstOrDefault();
        }
    }
}