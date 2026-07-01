using Ecom_Aplication.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IPriceHistory
    {
        Task<IEnumerable<PriceHistory_DTOS>> LISTAR_PRICEHISTORY_ASYNC();

        Task<IEnumerable<PriceHistory_DTOS>> FILTRAR_PRICEHISTORY_ASYNC(
            int? priceHistoryId,
            int? priceHistoryProductVariableId,
            decimal? priceHistoryOldPrice,
            decimal? priceHistoryNewPrice,
            DateTime? priceHistoryChangeDate,
            int? priceHistoryModifierId
        );

        Task<(int code, string message, int? templateId)> NUEVO_PRICEHISTORY_ASYNC(
            int priceHistoryProductVariableId,
            decimal priceHistoryOldPrice,
            decimal priceHistoryNewPrice,
            int? priceHistoryModifierId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_PRICEHISTORY_ASYNC(
            int priceHistoryId,
            int priceHistoryProductVariableId,
            decimal priceHistoryOldPrice,
            decimal priceHistoryNewPrice,
            int? priceHistoryModifierId
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_PRICEHISTORY_ASYNC(
            int priceHistoryId,
            int priceHistoryProductVariableId,
            decimal priceHistoryOldPrice,
            decimal priceHistoryNewPrice,
            int? priceHistoryModifierId
        );
    }
}
