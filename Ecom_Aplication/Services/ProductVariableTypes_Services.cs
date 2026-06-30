using Ecom_Aplication.Dtos; 
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class ProductVariableTypes_Services
    {
        private readonly IProductVariableTypesRepository _repository;

        public ProductVariableTypes_Services(IProductVariableTypesRepository repository)
        {
            _repository = repository;
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_PRODUCTVARIABLETYPES_ASYNC(ProductVariableTypes_DTOS dto)
        {
            return await _repository.NUEVO_PRODUCTVARIABLETYPES_ASYNC(
                dto.productVariableTypeName,
                dto.productVariableTypeDescription,
                dto.productVariableTypeCreatorId ?? 0,
                dto.productVariableTypeStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTVARIABLETYPES_ASYNC(ProductVariableTypes_DTOS dto)
        {
            return await _repository.ACTUALIZAR_PRODUCTVARIABLETYPES_ASYNC(
                dto.productVariableTypeId ?? 0,
                dto.productVariableTypeName,
                dto.productVariableTypeDescription,
                dto.productVariableTypeModificatorId ?? 0,
                dto.productVariableTypeStatusId ?? false,
                false 
            );
        }

        public async Task<IEnumerable<ProductVariableTypes_DTOS>> LISTAR_PRODUCTVARIABLETYPES()
        {
            var data = await _repository.LISTAR_PRODUCTVARIABLETYPES_ASYNC();

            return data.Select(t => new ProductVariableTypes_DTOS
            {
                productVariableTypeId = t.productVariableTypeId,
                productVariableTypeName = t.productVariableTypeName,
                productVariableTypeDescription = t.productVariableTypeDescription,
                productVariableTypeCreatorId = t.productVariableTypeCreatorId,
                productVariableTypeCreationDate = t.productVariableTypeCreationDate,
                productVariableTypeModificatorId = t.productVariableTypeModificatorId,
                productVariableTypeModificationDate = t.productVariableTypeModificationDate,
                productVariableTypeStatusId = t.productVariableTypeStatusId
            });
        }

        public async Task<ProductVariableTypes_DTOS?> Obtener_ProductVariableTypes_Por_Id(string searchTerm, bool? statusId)
        {
            var data = await _repository.FILTRAR_PRODUCTVARIABLETYPES_ASYNC(searchTerm, statusId);

            return data.Select(t => new ProductVariableTypes_DTOS
            {
                productVariableTypeId = t.productVariableTypeId,
                productVariableTypeName = t.productVariableTypeName,
                productVariableTypeDescription = t.productVariableTypeDescription,
                productVariableTypeCreatorId = t.productVariableTypeCreatorId,
                productVariableTypeCreationDate = t.productVariableTypeCreationDate,
                productVariableTypeModificatorId = t.productVariableTypeModificatorId,
                productVariableTypeModificationDate = t.productVariableTypeModificationDate,
                productVariableTypeStatusId = t.productVariableTypeStatusId
            }).FirstOrDefault();
        }

        public async Task<(int code, string message, int? templateId)> Eliminar_ProductVariableTypes(int productVariableTypeId, int productVariableTypeModificatorId)
        {
            return await _repository.ELIMINAR_PRODUCTVARIABLETYPES_ASYNC(productVariableTypeId, productVariableTypeModificatorId);
        }
    }
}