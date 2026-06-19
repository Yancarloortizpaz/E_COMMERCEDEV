using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IAttributeProductsRepository
    {
        Task<IEnumerable<AttributeProducts>> LISTAR_ATTRIBUTEPRODUCTS_ASYNC();

        Task<IEnumerable<AttributeProducts>> FILTRAR_ATTRIBUTEPRODUCTS_ASYNC(string searchTerm, bool? statusId);

        Task<(int code, string message, int? templateId)> NUEVO_ATTRIBUTEPRODUCTS_ASYNC(
            int AttributeProductAttributesTypeId,
            string AttributeProductName,
            string AttributeProductDescription,
            int AttributeProductCreatorId,
            bool AttributeProductStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_ATTRIBUTEPRODUCTS_ASYNC(
            int AttributeProductId,
            int AttributeProductAttributesTypeId,
            string AttributeProductName,
            string AttributeProductDescription,
            int AttributeProductModificatorId,
            bool AttributeProductStatusId,
            bool ForzarRecuperacion
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_ATTRIBUTEPRODUCTS_ASYNC(
            int AttributeProductId,
            int AttributeProductModificatorId
        );
    }
}