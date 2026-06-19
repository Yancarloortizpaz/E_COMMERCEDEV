using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface ICartsRepository
    {
        Task<IEnumerable<Carts>> LISTAR_CARTS_ASYNC();

        Task<IEnumerable<Carts>> FILTRAR_CARTS_ASYNC(string searchTerm, bool? statusId);

          Task<IEnumerable<Carts>> OBTENER_POR_USUARIO_CARTS_ASYNC(int cartUserId);

        Task<(int code, string message, int? templateId)> NUEVO_CARTS_ASYNC(
            int cartUserId,
            int cartCreatorId,
            bool cartStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_CARTS_ASYNC(
            int cartId,
            int cartUserId,
            int cartModificatorId,
            bool cartStatusId
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_CARTS_ASYNC(
            int cartId,
            int cartModificatorId
        );
    }
}