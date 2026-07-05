using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class PriceHistory_Services
    {
        private readonly IPriceHistory _priceHistoryRepository;

        public PriceHistory_Services(IPriceHistory priceHistoryRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
        }

        // 1. LISTAR
        public async Task<IEnumerable<PriceHistory_DTOS>> LISTAR_PRICEHISTORY_ASYNC()
        {
            return await _priceHistoryRepository.LISTAR_PRICEHISTORY_ASYNC();
        }

        public async Task<PriceHistory_DTOS?> OBTENER_PRICEHISTORY_BY_ID_ASYNC(int id)
        {
            var data = await _priceHistoryRepository.FILTRAR_PRICEHISTORY_ASYNC(id, null, null, null, null, null);
            return data.FirstOrDefault();
        }

        // 3. FILTRAR
        public async Task<IEnumerable<PriceHistory_DTOS>> FILTRAR_PRICEHISTORY_ASYNC(
            int? priceHistoryId,
            int? priceHistoryProductVariableId,
            decimal? priceHistoryOldPrice,
            decimal? priceHistoryNewPrice,
            DateTime? priceHistoryChangeDate,
            int? priceHistoryModifierId)
        {
            return await _priceHistoryRepository.FILTRAR_PRICEHISTORY_ASYNC(
                priceHistoryId,
                priceHistoryProductVariableId,
                priceHistoryOldPrice,
                priceHistoryNewPrice,
                priceHistoryChangeDate,
                priceHistoryModifierId
            );
        }
            public async Task<(int code, string message, int? templateId)> NUEVO_PRICEHISTORY_ASYNC(PriceHistory_DTOS dto)
        {
            return await _priceHistoryRepository.NUEVO_PRICEHISTORY_ASYNC(
                dto.priceHistoryProductVariableId ?? 0,
                dto.priceHistoryOldPrice ?? 0,
                dto.priceHistoryNewPrice ?? 0,
                dto.priceHistoryModifierId
            );
        }

        // 5. ACTUALIZAR
        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRICEHISTORY_ASYNC(PriceHistory_DTOS dto)
        {
            return await _priceHistoryRepository.ACTUALIZAR_PRICEHISTORY_ASYNC(
                dto.priceHistoryId ?? 0,
                dto.priceHistoryProductVariableId ?? 0,
                dto.priceHistoryOldPrice ?? 0,
                dto.priceHistoryNewPrice ?? 0,
                dto.priceHistoryModifierId
            );
        }

         public async Task<(int code, string message, int? templateId)> ELIMINAR_PRICEHISTORY_ASYNC(PriceHistory_DTOS dto)
        {
            return await _priceHistoryRepository.ELIMINAR_PRICEHISTORY_ASYNC(
                dto.priceHistoryId ?? 0,
                dto.priceHistoryProductVariableId ?? 0,
                dto.priceHistoryOldPrice ?? 0,
                dto.priceHistoryNewPrice ?? 0,
                dto.priceHistoryModifierId
            );
        }
    }
}