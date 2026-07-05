using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class UserPaymentMethods_Services : IUserPaymentMethods
    {
        private readonly IUserPaymentMethods _userPaymentMethodsRepository;

        public UserPaymentMethods_Services(IUserPaymentMethods userPaymentMethodsRepository)
        {
            _userPaymentMethodsRepository = userPaymentMethodsRepository;
        }

        public async Task<IEnumerable<UserPaymentMethods_DTOS>> LISTAR_USERPAYMENTMETHODS_ASYNC()
        {
            return await _userPaymentMethodsRepository.LISTAR_USERPAYMENTMETHODS_ASYNC();
        }

        public async Task<IEnumerable<UserPaymentMethods_DTOS>> FILTRAR_USERPAYMENTMETHODS_ASYNC(string searchTerm, bool? statusId)
        {
            return await _userPaymentMethodsRepository.FILTRAR_USERPAYMENTMETHODS_ASYNC(searchTerm, statusId);
        }

        // ¡CORREGIDO! Nombre sincronizado con el Controlador
        public async Task<IEnumerable<UserPaymentMethods_DTOS>> OBTENER_USERPAYMENTMETHOD_BY_ID_ASYNC(int id)
        {
            return await _userPaymentMethodsRepository.FILTRAR_USERPAYMENTMETHODS_ASYNC(id.ToString(), null);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_USERPAYMENTMETHODS_ASYNC(
            int userPaymentMethodUserId,
            int userPaymentMethodPaymentMethodTypeId,
            string cardNumberPlain,
            string expirationDatePlain,
            string cvvPlain,
            string userPaymentMethodCardHolderName,
            int userPaymentMethodCreatorId,
            bool userPaymentMethodStatusId)
        {
            return await _userPaymentMethodsRepository.NUEVO_USERPAYMENTMETHODS_ASYNC(
                userPaymentMethodUserId,
                userPaymentMethodPaymentMethodTypeId,
                cardNumberPlain,
                expirationDatePlain,
                cvvPlain,
                userPaymentMethodCardHolderName,
                userPaymentMethodCreatorId,
                userPaymentMethodStatusId
            );
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_USERPAYMENTMETHODS_ASYNC(UserPaymentMethods_DTOS dto)
        {
            return await _userPaymentMethodsRepository.NUEVO_USERPAYMENTMETHODS_ASYNC(
                dto.userPaymentMethodUserId ?? 0,
                dto.userPaymentMethodPaymentMethodTypeId ?? 0,
                dto.userPaymentMethodCardNumber != null ? System.Text.Encoding.UTF8.GetString(dto.userPaymentMethodCardNumber) : "",
                dto.userPaymentMethodExpirationDate != null ? System.Text.Encoding.UTF8.GetString(dto.userPaymentMethodExpirationDate) : "",
                dto.userPaymentMethodCVV != null ? System.Text.Encoding.UTF8.GetString(dto.userPaymentMethodCVV) : "",
                dto.userPaymentMethodCardHolderName ?? "",
                dto.userPaymentMethodCreatorId ?? 0,
                dto.userPaymentMethodStatusId ?? false
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_USERPAYMENTMETHODS_ASYNC(
            int userPaymentMethodId,
            int userPaymentMethodPaymentMethodTypeId,
            string cardNumberPlain,
            string expirationDatePlain,
            string cvvPlain,
            string userPaymentMethodCardHolderName,
            int userPaymentMethodModificatorId,
            bool userPaymentMethodStatusId,
            bool forzarRecuperacion)
        {
            return await _userPaymentMethodsRepository.ACTUALIZAR_USERPAYMENTMETHODS_ASYNC(
                userPaymentMethodId,
                userPaymentMethodPaymentMethodTypeId,
                cardNumberPlain,
                expirationDatePlain,
                cvvPlain,
                userPaymentMethodCardHolderName,
                userPaymentMethodModificatorId,
                userPaymentMethodStatusId,
                forzarRecuperacion
            );
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_USERPAYMENTMETHODS_ASYNC(UserPaymentMethods_DTOS dto)
        {
            return await _userPaymentMethodsRepository.ACTUALIZAR_USERPAYMENTMETHODS_ASYNC(
                dto.userPaymentMethodId ?? 0,
                dto.userPaymentMethodPaymentMethodTypeId ?? 0,
                dto.userPaymentMethodCardNumber != null ? System.Text.Encoding.UTF8.GetString(dto.userPaymentMethodCardNumber) : "",
                dto.userPaymentMethodExpirationDate != null ? System.Text.Encoding.UTF8.GetString(dto.userPaymentMethodExpirationDate) : "",
                dto.userPaymentMethodCVV != null ? System.Text.Encoding.UTF8.GetString(dto.userPaymentMethodCVV) : "",
                dto.userPaymentMethodCardHolderName ?? "",
                dto.userPaymentMethodModificatorId ?? 0,
                dto.userPaymentMethodStatusId ?? false,
                false
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_USERPAYMENTMETHODS_ASYNC(
            int userPaymentMethodId,
            int userPaymentMethodModificatorId)
        {
            return await _userPaymentMethodsRepository.ELIMINAR_USERPAYMENTMETHODS_ASYNC(userPaymentMethodId, userPaymentMethodModificatorId);
        }
    }
}