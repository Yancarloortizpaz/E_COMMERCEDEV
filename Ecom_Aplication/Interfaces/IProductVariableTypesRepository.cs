using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IProductVariableTypesRepository
    {
        Task<IEnumerable<ProductVariableTypes>> LISTAR_PRODUCTVARIABLETYPES_ASYNC();

        Task<IEnumerable<ProductVariableTypes>> FILTRAR_PRODUCTVARIABLETYPES_ASYNC(string searchTerm, bool? statusId);

        Task<(int code, string message, int? templateId)> NUEVO_PRODUCTVARIABLETYPES_ASYNC(
            string productVariableTypeName,
            string productVariableTypeDescription,
            int productVariableTypeCreatorId,
            bool productVariableTypeStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTVARIABLETYPES_ASYNC(
            int productVariableTypeId,
            string productVariableTypeName,
            string productVariableTypeDescription,
            int productVariableTypeModificatorId,
            bool productVariableTypeStatusId,
            bool ForzarRecuperacion
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTVARIABLETYPES_ASYNC(
            int productVariableTypeId,
            int productVariableTypeModificatorId
        );
    }
}