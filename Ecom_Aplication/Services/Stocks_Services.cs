using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class Stocks_Services
    {
        private readonly IStocksRepository _repository;

        public Stocks_Services(IStocksRepository repository)
        {
            _repository = repository;
        }

        public async Task<(int code, string message, bool? templateId)> NUEVO_STOCK_ASYNC(Stocks_DTOS dto)
        {
            return await _repository.NUEVO_STOCK_ASYNC(
                dto.stockproductvariableid ?? 0,
                dto.stockquantity ?? 0,
                dto.stockcreatorid ?? 0,
                dto.stockstatusid ?? false
            );
        }

        public async Task<(int code, string message, bool? templateId)> ACTUALIZAR_STOCK_ASYNC(Stocks_DTOS dto)
        {
            return await _repository.ACTUALIZAR_STOCK_ASYNC(
                dto.stockid ?? 0,
                dto.stockproductvariableid ?? 0,
                dto.stockquantity ?? 0,
                dto.stockmodificatorid ?? 0,
                dto.stockstatusid ?? false,
                dto.forzarrecuperacion ?? false
            );
        }

        public async Task<IEnumerable<Stocks_DTOS>> LISTAR_STOCK()
        {
            var data = await _repository.LISTAR_STOCK_ASYNC();

            return data.Select(s => new Stocks_DTOS
            {
                stockid = s.StockId,
                stockproductvariableid = s.StockProductVariableId,
                stockquantity = s.StockQuantity,
                stockcreatorid = s.StockCreatorId,
                stockcreationdate = s.StockCreationDate,
                stockmodificatorid = s.StockModificatorId,
                stockmodificationdate = s.StockModificationDate,
                stockstatusid = s.StockStatusId
            });
        }

        public async Task<Stocks_DTOS?> Obtener_Stock_Por_Id(string searchTerm, bool? statusId)
        {
            var data = await _repository.FILTRAR_STOCK_ASYNC(searchTerm, statusId);

            return data.Select(s => new Stocks_DTOS
            {
                Stockid = s.StockId,
                Stockproductvariableid = s.StockProductVariableId,
                Stockquantity = s.StockQuantity,
                Stockcreatorid = s.StockCreatorId,
                Stockcreationdate = s.StockCreationDate,
                Stockmodificatorid = s.StockModificatorId,
                Stockmodificationdate = s.StockModificationDate,
                Stockstatusid = s.StockStatusId
            }).FirstOrDefault();
        }

        public async Task<IEnumerable<Stocks_DTOS>> Obtener_Por_Variable_Producto(int productVariableId)
        {
            var data = await _repository.OBTENER_POR_VARIABLE_PRODUCTVARIABLES_ASYNC(productVariableId);

            return data.Select(s => new Stocks_DTOS
            {
                stockid = s.StockId,
                stockproductvariableid = s.StockProductVariableId,
                stockquantity = s.StockQuantity,
                stockcreatorid = s.StockCreatorId,
                stockcreationdate = s.StockCreationDate,
                stockmodificatorid = s.StockModificatorId,
                stockmodificationdate = s.StockModificationDate,
                stockstatusid = s.StockStatusId
            });
        }

        public async Task<(int code, string message, bool? templateId)> Eliminar_Stock(int stockId)
        {
            return await _repository.ELIMINAR_STOCK_ASYNC(stockId);
        }
    }
}