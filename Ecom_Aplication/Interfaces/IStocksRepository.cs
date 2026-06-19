using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IStocksRepository
    {
        Task<IEnumerable<Stocks>> LISTAR_STOCKS_ASYNC();

        Task<IEnumerable<Stocks>> FILTRAR_STOCKS_ASYNC(string searchTerm, bool? statusId);

        Task<IEnumerable<Stocks>> OBTENER_POR_PRODUCTVARIABLE_STOCKS_ASYNC(int ProductVariableId);

        Task<(int code, string message, int? templateId)> NUEVO_STOCKS_ASYNC(
            int stockProductVariableId,
            int stockQuantity,
            DateTime stockFactoryDate,
            DateTime stockExpirationDate,
            int stockCreatorId,
            bool stockStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_STOCKS_ASYNC(
            int stockId,
            int stockQuantityAdjustment,
            int stockModificatorId
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_STOCKS_ASYNC(
            int stockId,
            int stockModificatorId
        );
    }
}