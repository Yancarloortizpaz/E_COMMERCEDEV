using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace modu.application.Interface
{
    public interface IUserAddressRepository
    {

        Task<IEnumerable<UserAddress>> LISTAR_USER_ADDRESS_ASYNC();
        Task<IEnumerable<UserAddress>> FILTRAR_USER_ADDRESS_ASYNC(int? userId, string searchTerm, bool? statusId);
        Task<(int code, string message, int? templateId)> NUEVO_USER_ADDRESS_ASYNC(
            int userAddressUserId,
            int userAddressCountryId,
            int userAddressZIPCode,
            string userAddressDescription,
            bool userAddressIsPrincipal,
            int userAddressCreatorId,
            bool userAddressStatusId
        );
        Task<(int code, string message, int? templateId)> ACTUALIZAR_USER_ADDRESS_ASYNC(
            int userAddressId,
            int userAddressCountryId,
            int userAddressZIPCode,
            string userAddressDescription,
            bool userAddressIsPrincipal,
            int userAddressModificatorId,
            bool userAddressStatusId,
            bool forzarRecuperacion
        );
        Task<(int code, string message, int? templateId)> ELIMINAR_USER_ADDRESS_ASYNC(int userAddressId, int userAddressModificatorId);
    }
}