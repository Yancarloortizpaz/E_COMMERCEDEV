using Ecom_Domain;
using modu.application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class UserAddress_Services : IUserAddressRepository
    {
        private readonly IUserAddressRepository _userAddressRepository;

        public UserAddress_Services(IUserAddressRepository userAddressRepository)
        {
            _userAddressRepository = userAddressRepository;
        }

        public async Task<IEnumerable<UserAddress>> LISTAR_USER_ADDRESS_ASYNC()
        {
            return await _userAddressRepository.LISTAR_USER_ADDRESS_ASYNC();
        }

        public async Task<IEnumerable<UserAddress>> FILTRAR_USER_ADDRESS_ASYNC(int? userId, string searchTerm, bool? statusId)
        {
            return await _userAddressRepository.FILTRAR_USER_ADDRESS_ASYNC(userId, searchTerm, statusId);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_USER_ADDRESS_ASYNC(
            int userAddressUserId,
            int userAddressCountryId,
            int userAddressZIPCode,
            string userAddressDescription,
            bool userAddressIsPrincipal,
            int userAddressCreatorId,
            bool userAddressStatusId)
        {
            return await _userAddressRepository.NUEVO_USER_ADDRESS_ASYNC(
                userAddressUserId,
                userAddressCountryId,
                userAddressZIPCode,
                userAddressDescription,
                userAddressIsPrincipal,
                userAddressCreatorId,
                userAddressStatusId
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_USER_ADDRESS_ASYNC(
            int userAddressId,
            int userAddressCountryId,
            int userAddressZIPCode,
            string userAddressDescription,
            bool userAddressIsPrincipal,
            int userAddressModificatorId,
            bool userAddressStatusId,
            bool forzarRecuperacion)
        {
            return await _userAddressRepository.ACTUALIZAR_USER_ADDRESS_ASYNC(
                userAddressId,
                userAddressCountryId,
                userAddressZIPCode,
                userAddressDescription,
                userAddressIsPrincipal,
                userAddressModificatorId,
                userAddressStatusId,
                forzarRecuperacion
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_USER_ADDRESS_ASYNC(int userAddressId, int userAddressModificatorId)
        {
            return await _userAddressRepository.ELIMINAR_USER_ADDRESS_ASYNC(userAddressId, userAddressModificatorId);
        }
    }
}