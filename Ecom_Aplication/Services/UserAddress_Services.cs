using Ecom_Aplication.Dtos;
using Ecom_Domain;
using modu.application.Interface;

namespace Ecom_Aplication.Services
{
    public class UserAddress_Services
    {
        private readonly IUserAddressRepository _userAddressRepository;

        public UserAddress_Services(IUserAddressRepository userAddressRepository)
        {
            _userAddressRepository = userAddressRepository;
        }

        public async Task<IEnumerable<UserAddress>> LISTAR_USERADDRESS_ASYNC()
        {
            return await _userAddressRepository.LISTAR_USER_ADDRESS_ASYNC();
        }

        public async Task<UserAddress?> OBTENER_USERADDRESS_BY_ID_ASYNC(int id)
        {
            var data = await _userAddressRepository.FILTRAR_USER_ADDRESS_ASYNC(null, id.ToString(), null);
            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<UserAddress>> FILTRAR_USERADDRESS_ASYNC(int userId)
        {
            return await _userAddressRepository.FILTRAR_USER_ADDRESS_ASYNC(userId, "", null);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_USERADDRESS_ASYNC(UserAddress_DTOS dto)
        {
            return await _userAddressRepository.NUEVO_USER_ADDRESS_ASYNC(
                dto.UserAddressUserId ?? 0,
                dto.UserAddressCountryId ?? 0,
                dto.UserAddressZIPCode ?? 0,
                dto.UserAddressDescription ?? "",
                dto.UserAddressIsPrincipal ?? false,
                dto.UserAddressCreatorId ?? 0,
                dto.UserAddressStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_USERADDRESS_ASYNC(UserAddress_DTOS dto)
        {
            return await _userAddressRepository.ACTUALIZAR_USER_ADDRESS_ASYNC(
                dto.UserAddressId ?? 0,
                dto.UserAddressCountryId ?? 0,
                dto.UserAddressZIPCode ?? 0,
                dto.UserAddressDescription ?? "",
                dto.UserAddressIsPrincipal ?? false,
                dto.UserAddressModificatorId ?? 0,
                dto.UserAddressStatusId ?? false,
                false
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_USERADDRESS_ASYNC(int id, int modificatorId)
        {
            return await _userAddressRepository.ELIMINAR_USER_ADDRESS_ASYNC(id, modificatorId);
        }
    }
}