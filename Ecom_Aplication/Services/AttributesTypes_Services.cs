using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class AttributesType_Services
    {
        private readonly IAttributesType _attributesTypeRepository;

        public AttributesType_Services(IAttributesType attributesTypeRepository)
        {
            _attributesTypeRepository = attributesTypeRepository;
        }

        public async Task<IEnumerable<AttributesTypes_DTOS>> LISTAR_ATTRIBUTESTYPES_ASYNC()
        {
            return await _attributesTypeRepository.LISTAR_ATTRIBUTESTYPE_ASYNC();
        }

        public async Task<AttributesTypes_DTOS?> OBTENER_ATTRIBUTESTYPE_BY_ID_ASYNC(int id)
        {
            var data = await _attributesTypeRepository.FILTRAR_ATTRIBUTESTYPE_ASYNC(id, "", "", null, null, null, null, null);
            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<AttributesTypes_DTOS>> FILTRAR_ATTRIBUTESTYPE_ASYNC(
            int? attributeTypeId,
            string? attributeTypeName,
            string? attributeTypeDescription,
            int? attributeTypeCreatorId,
            DateTime? attributeTypeCreationDate,
            int? attributeTypeModificatorId,
            DateTime? attributeTypeModificationDate,
            bool? attributeTypeStatusId)
        {
            return await _attributesTypeRepository.FILTRAR_ATTRIBUTESTYPE_ASYNC(
                attributeTypeId,
                attributeTypeName ?? "",
                attributeTypeDescription ?? "",
                attributeTypeCreatorId,
                attributeTypeCreationDate,
                attributeTypeModificatorId,
                attributeTypeModificationDate,
                attributeTypeStatusId
            );
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_ATTRIBUTESTYPE_ASYNC(AttributesTypes_DTOS dto)
        {
            return await _attributesTypeRepository.NUEVO_ATTRIBUTESTYPE_ASYNC(
                dto.AttributeTypeName ?? "",
                dto.AttributeTypeDescription ?? "",
                dto.AttributeTypeCreatorId ?? 0,
                dto.AttributeTypeModificatorId,
                dto.AttributeTypeStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_ATTRIBUTESTYPE_ASYNC(AttributesTypes_DTOS dto)
        {
            return await _attributesTypeRepository.ACTUALIZAR_ATTRIBUTESTYPE_ASYNC(
                dto.AttributeTypeId ?? 0,
                dto.AttributeTypeName ?? "",
                dto.AttributeTypeDescription ?? "",
                dto.AttributeTypeCreatorId ?? 0,
                dto.AttributeTypeModificatorId ?? 0,
                dto.AttributeTypeStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_ATTRIBUTESTYPE_ASYNC(AttributesTypes_DTOS dto)
        {
            return await _attributesTypeRepository.ELIMINAR_ATTRIBUTESTYPE_ASYNC(
                dto.AttributeTypeId ?? 0,
                dto.AttributeTypeName ?? "",
                dto.AttributeTypeDescription ?? "",
                dto.AttributeTypeCreatorId ?? 0,
                dto.AttributeTypeModificatorId ?? 0,
                dto.AttributeTypeStatusId ?? false
            );
        }
    }
}