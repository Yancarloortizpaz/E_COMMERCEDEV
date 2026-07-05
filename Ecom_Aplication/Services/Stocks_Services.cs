using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class Stocks_Services
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

        public async Task<Stocks?> OBTENER_STOCK_BY_ID_ASYNC(int id)
        {
            var data = await _stocksRepository.FILTRAR_STOCKS_ASYNC(id.ToString(), null);
            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<Stocks>> FILTRAR_STOCKS_ASYNC(string searchTerm, bool? statusId)
        {
            return await _stocksRepository.FILTRAR_STOCKS_ASYNC(searchTerm ?? "", statusId);
        }

        public async Task<IEnumerable<Stocks>> OBTENER_POR_PRODUCTVARIABLE_STOCKS_ASYNC(int productVariableId)
        {
            return await _stocksRepository.OBTENER_POR_PRODUCTVARIABLE_STOCKS_ASYNC(productVariableId);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_STOCKS_ASYNC(Stocks_DTOS dto)
        {
            return await _stocksRepository.NUEVO_STOCKS_ASYNC(
                dto.stockProductVariableId ?? 0,
                dto.stockQuantity ?? 0,
                dto.stockFactoryDate ?? DateTime.Now,
                dto.stockExpirationDate ?? DateTime.Now.AddYears(1),
                dto.stockCreatorId ?? 0,
                dto.stockStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_STOCKS_ASYNC(Stocks_DTOS dto)
        {
            return await _stocksRepository.ACTUALIZAR_STOCKS_ASYNC(
                dto.stockId ?? 0,
                dto.stockQuantity ?? 0, // Mapeado al parámetro de ajuste/cantidad
                dto.stockModificatorId ?? 0
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_STOCKS_ASYNC(int stockId, int stockModificatorId)
        {
            return await _stocksRepository.ELIMINAR_STOCKS_ASYNC(stockId, stockModificatorId);
        }
    }
}