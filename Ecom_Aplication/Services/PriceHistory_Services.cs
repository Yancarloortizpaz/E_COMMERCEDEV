using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class PriceHistory_Services : IPriceHistory
    {
        private readonly IPriceHistory _priceHistoryRepository;

        public PriceHistory_Services(IPriceHistory priceHistoryRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
        }

        public async Task<IEnumerable<PriceHistory_DTOS>> LISTAR_PRICEHISTORY_ASYNC()
        {
            return await _priceHistoryRepository.LISTAR_PRICEHISTORY_ASYNC();
        }

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

        public async Task<(int code, string message, int? templateId)> NUEVO_PRICEHISTORY_ASYNC(
            int priceHistoryProductVariableId,
            decimal priceHistoryOldPrice,
            decimal priceHistoryNewPrice,
            int? priceHistoryModifierId)
        {
            return await _priceHistoryRepository.NUEVO_PRICEHISTORY_ASYNC(
                priceHistoryProductVariableId,
                priceHistoryOldPrice,
                priceHistoryNewPrice,
                priceHistoryModifierId
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRICEHISTORY_ASYNC(
            int priceHistoryId,
            int priceHistoryProductVariableId,
            decimal priceHistoryOldPrice,
            decimal priceHistoryNewPrice,
            int? priceHistoryModifierId)
        {
            return await _priceHistoryRepository.ACTUALIZAR_PRICEHISTORY_ASYNC(
                priceHistoryId,
                priceHistoryProductVariableId,
                priceHistoryOldPrice,
                priceHistoryNewPrice,
                priceHistoryModifierId
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_PRICEHISTORY_ASYNC(
            int priceHistoryId,
            int priceHistoryProductVariableId,
            decimal priceHistoryOldPrice,
            decimal priceHistoryNewPrice,
            int? priceHistoryModifierId)
        {
            return await _priceHistoryRepository.ELIMINAR_PRICEHISTORY_ASYNC(
                priceHistoryId,
                priceHistoryProductVariableId,
                priceHistoryOldPrice,
                priceHistoryNewPrice,
                priceHistoryModifierId
            );
        }
    }
}