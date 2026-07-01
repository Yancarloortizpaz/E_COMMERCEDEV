using Ecom_Aplication.Interfaces;
using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class Stocks_Services : IStocksRepository
    {
        private readonly IStocksRepository _stocksRepository;

        public Stocks_Services(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }

        public async Task<IEnumerable<Stocks>> LISTAR_STOCKS_ASYNC()
        {
            return await _stocksRepository.LISTAR_STOCKS_ASYNC();
        }

        public async Task<IEnumerable<Stocks>> FILTRAR_STOCKS_ASYNC(string searchTerm, bool? statusId)
        {
            return await _stocksRepository.FILTRAR_STOCKS_ASYNC(searchTerm, statusId);
        }

        public async Task<IEnumerable<Stocks>> OBTENER_POR_PRODUCTVARIABLE_STOCKS_ASYNC(int ProductVariableId)
        {
            return await _stocksRepository.OBTENER_POR_PRODUCTVARIABLE_STOCKS_ASYNC(ProductVariableId);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_STOCKS_ASYNC(
            int stockProductVariableId,
            int stockQuantity,
            DateTime stockFactoryDate,
            DateTime stockExpirationDate,
            int stockCreatorId,
            bool stockStatusId)
        {
            return await _stocksRepository.NUEVO_STOCKS_ASYNC(
                stockProductVariableId,
                stockQuantity,
                stockFactoryDate,
                stockExpirationDate,
                stockCreatorId,
                stockStatusId
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_STOCKS_ASYNC(
            int stockId,
            int stockQuantityAdjustment,
            int stockModificatorId)
        {
            return await _stocksRepository.ACTUALIZAR_STOCKS_ASYNC(
                stockId,
                stockQuantityAdjustment,
                stockModificatorId
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_STOCKS_ASYNC(
            int stockId,
            int stockModificatorId)
        {
            return await _stocksRepository.ELIMINAR_STOCKS_ASYNC(stockId, stockModificatorId);
        }
    }
}