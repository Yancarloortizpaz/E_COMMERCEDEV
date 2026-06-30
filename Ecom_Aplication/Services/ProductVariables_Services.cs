using Ecom_Aplication.Dtos; 
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class ProductVariables_Services
    {
        private readonly IProductVariablesRepository _repository;

        public ProductVariables_Services(IProductVariablesRepository repository)
        {
            _repository = repository;
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_PRODUCTVARIABLES_ASYNC(ProductVariables_DTOS dto)
        {
            return await _repository.NUEVO_PRODUCTVARIABLES_ASYNC(
                dto.productVariableProductId ?? 0,
                dto.productVariableValue,
                dto.productVariablePrice ?? 0,
                dto.productVariableCurrencyId ?? 0,
                dto.productVariableCreatorId ?? 0,
                dto.productVariableStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTVARIABLES_ASYNC(ProductVariables_DTOS dto)
        {
            return await _repository.ACTUALIZAR_PRODUCTVARIABLES_ASYNC(
                dto.productVariableId ?? 0,
                dto.productVariableProductId ?? 0,
                dto.productVariableValue,
                dto.productVariablePrice ?? 0,
                dto.productVariableCurrencyId ?? 0,
                dto.productVariableModificatorId ?? 0,
                dto.productVariableStatusId ?? false,
                false
            );
        }

        public async Task<IEnumerable<ProductVariables_DTOS>> LISTAR_PRODUCTVARIABLES()
        {
            var data = await _repository.LISTAR_PRODUCTVARIABLES_ASYNC();

            return data.Select(v => new ProductVariables_DTOS
            {
                productVariableId = v.productVariableId,
                productVariableProductId = v.productVariableProductId,
                productVariableValue = v.productVariableValue,
                productVariablePrice = v.productVariablePrice,
                productVariableCurrencyId = v.productVariableCurrencyId,
                productVariableCreatorId = v.productVariableCreatorId,
                productVariableCreationDate = v.productVariableCreationDate,
                productVariableModificatorId = v.productVariableModificatorId,
                productVariableModificationDate = v.productVariableModificationDate,
                productVariableStatusId = v.productVariableStatusId
            });
        }

        public async Task<ProductVariables_DTOS?> Obtener_ProductVariables_Por_Id(string searchTerm, bool? statusId)
        {
            var data = await _repository.FILTRAR_PRODUCTVARIABLES_ASYNC(searchTerm, statusId);

            return data.Select(v => new ProductVariables_DTOS
            {
                productVariableId = v.productVariableId,
                productVariableProductId = v.productVariableProductId,
                productVariableValue = v.productVariableValue,
                productVariablePrice = v.productVariablePrice,
                productVariableCurrencyId = v.productVariableCurrencyId,
                productVariableCreatorId = v.productVariableCreatorId,
                productVariableCreationDate = v.productVariableCreationDate,
                productVariableModificatorId = v.productVariableModificatorId,
                productVariableModificationDate = v.productVariableModificationDate,
                productVariableStatusId = v.productVariableStatusId
            }).FirstOrDefault();
        }

        public async Task<(int code, string message, int? templateId)> Eliminar_ProductVariables(int productVariableId, int productVariableModificatorId)
        {
            return await _repository.ELIMINAR_PRODUCTVARIABLES_ASYNC(productVariableId, productVariableModificatorId);
        }
    }
}