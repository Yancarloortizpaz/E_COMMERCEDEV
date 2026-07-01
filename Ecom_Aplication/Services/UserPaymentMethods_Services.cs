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

        public async Task<(int code, string message, int? templateId)> ELIMINAR_USERPAYMENTMETHODS_ASYNC(
            int userPaymentMethodId,
            int userPaymentMethodModificatorId)
        {
            return await _userPaymentMethodsRepository.ELIMINAR_USERPAYMENTMETHODS_ASYNC(userPaymentMethodId, userPaymentMethodModificatorId);
        }
    }
}
