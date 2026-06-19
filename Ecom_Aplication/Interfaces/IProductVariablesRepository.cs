using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IProductVariablesRepository
    {
        Task<IEnumerable<ProductVariables>> LISTAR_PRODUCTVARIABLES_ASYNC();

        Task<IEnumerable<ProductVariables>> FILTRAR_PRODUCTVARIABLES_ASYNC(string searchTerm, bool? statusId);

        Task<(int code, string message, int? templateId)> NUEVO_PRODUCTVARIABLES_ASYNC(
            int productVariableProductId,
            string productVariableValue,
            decimal productVariablePrice,
            int productVariableCurrencyId,
            int productVariableCreatorId,
            bool productVariableStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTVARIABLES_ASYNC(
            int productVariableId,
            int productVariableProductId,
            string productVariableValue,
            decimal productVariablePrice,
            int productVariableCurrencyId,
            int productVariableModificatorId,
            bool productVariableStatusId,
            bool ForzarRecuperacion
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTVARIABLES_ASYNC(
            int productVariableId,
            int productVariableModificatorId
        );
    }
}