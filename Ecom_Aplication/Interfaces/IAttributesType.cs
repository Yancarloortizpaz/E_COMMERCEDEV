using Ecom_Aplication.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IAttributesType
    {
        Task<IEnumerable<AttributesTypes_DTOS>> LISTAR_ATTRIBUTESTYPE_ASYNC();

        Task<IEnumerable<AttributesTypes_DTOS>> FILTRAR_ATTRIBUTESTYPE_ASYNC(
            int? attributeTypeId,
            string attributeTypeName,
            string attributeTypeDescription,
            int? attributeTypeCreatorId,
            DateTime? attributeTypeCreationDate,
            int? attributeTypeModificatorId,
            DateTime? attributeTypeModificationDate,
            bool? attributeTypeStatusId
        );

        Task<(int code, string message, int? templateId)> NUEVO_ATTRIBUTESTYPE_ASYNC(
            string attributeTypeName,
            string attributeTypeDescription,
            int attributeTypeCreatorId,
            int? attributeTypeModificatorId,
            bool attributeTypeStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_ATTRIBUTESTYPE_ASYNC(
            int attributeTypeId,
            string attributeTypeName,
            string attributeTypeDescription,
            int attributeTypeCreatorId,
            int attributeTypeModificatorId,
            bool attributeTypeStatusId
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_ATTRIBUTESTYPE_ASYNC(
            int attributeTypeId,
            string attributeTypeName,
            string attributeTypeDescription,
            int attributeTypeCreatorId,
            int attributeTypeModificatorId,
            bool attributeTypeStatusId
        );
    }
}