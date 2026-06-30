using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class AttributeProducts_Services
    {
        private readonly IAttributeProductsRepository _repository;

        public AttributeProducts_Services(IAttributeProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_ATTRIBUTEPRODUCTS_ASYNC(AttributeProducts_DTOS dto)
        {
            return await _repository.NUEVO_ATTRIBUTEPRODUCTS_ASYNC(
                dto.AttributeProductAttributesTypeId ?? 0,
                dto.AttributeProductName,
                dto.AttributeProductDescription,
                dto.AttributeProductCreatorId ?? 0,
                dto.AttributeProductStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_ATTRIBUTEPRODUCTS_ASYNC(AttributeProducts_DTOS dto)
        {
            return await _repository.ACTUALIZAR_ATTRIBUTEPRODUCTS_ASYNC(
                dto.AttributeProductId ?? 0,
                dto.AttributeProductAttributesTypeId ?? 0,
                dto.AttributeProductName,
                dto.AttributeProductDescription,
                dto.AttributeProductModificatorId ?? 0,
                dto.AttributeProductStatusId ?? false,
                false // Por defecto mandamos false para ForzarRecuperacion
            );
        }

        public async Task<IEnumerable<AttributeProducts_DTOS>> LISTAR_ATTRIBUTEPRODUCTS()
        {
            var data = await _repository.LISTAR_ATTRIBUTEPRODUCTS_ASYNC();

            return data.Select(p => new AttributeProducts_DTOS
            {
                AttributeProductId = p.AttributeProductId,
                AttributeProductAttributesTypeId = p.AttributeProductAttributesTypeId,
                AttributeProductName = p.AttributeProductName,
                AttributeProductDescription = p.AttributeProductDescription,
                AttributeProductCreatorId = p.AttributeProductCreatorId,
                AttributeProductCreationDate = p.AttributeProductCreationDate,
                AttributeProductModificatorId = p.AttributeProductModificatorId,
                AttributeProductModificationDate = p.AttributeProductModificationDate,
                AttributeProductStatusId = p.AttributeProductStatusId
            });
        }

        public async Task<AttributeProducts_DTOS?> Obtener_AttributeProducts_Por_Id(string searchTerm, bool? statusId)
        {
            var data = await _repository.FILTRAR_ATTRIBUTEPRODUCTS_ASYNC(searchTerm, statusId);

            return data.Select(p => new AttributeProducts_DTOS
            {
                AttributeProductId = p.AttributeProductId,
                AttributeProductAttributesTypeId = p.AttributeProductAttributesTypeId,
                AttributeProductName = p.AttributeProductName,
                AttributeProductDescription = p.AttributeProductDescription,
                AttributeProductCreatorId = p.AttributeProductCreatorId,
                AttributeProductCreationDate = p.AttributeProductCreationDate,
                AttributeProductModificatorId = p.AttributeProductModificatorId,
                AttributeProductModificationDate = p.AttributeProductModificationDate,
                AttributeProductStatusId = p.AttributeProductStatusId
            }).FirstOrDefault();
        }

        public async Task<(int code, string message, int? templateId)> Eliminar_AttributeProducts(int attributeProductId, int attributeProductModificatorId)
        {
            return await _repository.ELIMINAR_ATTRIBUTEPRODUCTS_ASYNC(attributeProductId, attributeProductModificatorId);
        }
    }
}
