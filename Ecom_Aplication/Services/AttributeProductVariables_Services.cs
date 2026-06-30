using Ecom_Aplication.Dtos; 
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class AttributeProductVariables_Services
    {
        private readonly IAttributeProductVariablesRepository _repository;

        public AttributeProductVariables_Services(IAttributeProductVariablesRepository repository)
        {
            _repository = repository;
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_ATTRIBUTEPRODUCTVARIABLES_ASYNC(AttributeProductVariables_DTOS dto)
        {
            return await _repository.NUEVO_ATTRIBUTEPRODUCTVARIABLES_ASYNC(
                dto.attributeProductVariableProductVariableId ?? 0,
                dto.attributeProductVariableAttributeProductId ?? 0,
                dto.attributeProductVariableValue,
                dto.attributeProductVariableCreatorId ?? 0,
                dto.attributeProductVariableStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_ATTRIBUTEPRODUCTVARIABLES_ASYNC(AttributeProductVariables_DTOS dto)
        {
            return await _repository.ACTUALIZAR_ATTRIBUTEPRODUCTVARIABLES_ASYNC(
                dto.attributeProductVariableId ?? 0,
                dto.attributeProductVariableProductVariableId ?? 0,
                dto.attributeProductVariableAttributeProductId ?? 0,
                dto.attributeProductVariableValue,
                dto.attributeProductVariableModificatorId ?? 0,
                dto.attributeProductVariableStatusId ?? false,
                false 
            );
        }

        public async Task<IEnumerable<AttributeProductVariables_DTOS>> OBTENER_POR_PRODUCTVARIABLE(int productVariableId)
        {
            var data = await _repository.OBTENER_POR_PRODUCTVARIABLE_ATTRIBUTEPRODUCTVARIABLES_ASYNC(productVariableId);

            return data.Select(p => new AttributeProductVariables_DTOS
            {
                attributeProductVariableId = p.attributeProductVariableId,
                attributeProductVariableProductVariableId = p.attributeProductVariableProductVariableId,
                attributeProductVariableAttributeProductId = p.attributeProductVariableAttributeProductId,
                attributeProductVariableValue = p.attributeProductVariableValue,
                attributeProductVariableCreatorId = p.attributeProductVariableCreatorId,
                attributeProductVariableCreationDate = p.attributeProductVariableCreationDate,
                attributeProductVariableModificatorId = p.attributeProductVariableModificatorId,
                attributeProductVariableModificationDate = p.attributeProductVariableModificationDate,
                attributeProductVariableStatusId = p.attributeProductVariableStatusId
            });
        }

        public async Task<(int code, string message, int? templateId)> Eliminar_AttributeProductVariables(int attributeProductVariableId, int attributeProductVariableModificatorId)
        {
            return await _repository.ELIMINAR_ATTRIBUTEPRODUCTVARIABLES_ASYNC(attributeProductVariableId, attributeProductVariableModificatorId);
        }
    }
}
