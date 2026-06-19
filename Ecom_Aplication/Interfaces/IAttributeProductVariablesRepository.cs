using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IAttributeProductVariablesRepository
    {
        Task<IEnumerable<AttributeProductVariables>> OBTENER_POR_PRODUCTVARIABLE_ATTRIBUTEPRODUCTVARIABLES_ASYNC(int ProductVariableId);

        Task<(int code, string message, int? templateId)> NUEVO_ATTRIBUTEPRODUCTVARIABLES_ASYNC(
            int attributeProductVariableProductVariableId,
            int attributeProductVariableAttributeProductId,
            string attributeProductVariableValue,
            int attributeProductVariableCreatorId,
            bool attributeProductVariableStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_ATTRIBUTEPRODUCTVARIABLES_ASYNC(
            int attributeProductVariableId,
            int attributeProductVariableProductVariableId,
            int attributeProductVariableAttributeProductId,
            string attributeProductVariableValue,
            int attributeProductVariableModificatorId,
            bool attributeProductVariableStatusId,
            bool ForzarRecuperacion
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_ATTRIBUTEPRODUCTVARIABLES_ASYNC(
            int attributeProductVariableId,
            int attributeProductVariableModificatorId
        );
    }
}