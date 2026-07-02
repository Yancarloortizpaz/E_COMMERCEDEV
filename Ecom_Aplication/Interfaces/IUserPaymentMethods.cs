using Ecom_Aplication.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IUserPaymentMethods
    {
        Task<IEnumerable<UserPaymentMethods_DTOS>> LISTAR_USERPAYMENTMETHODS_ASYNC();

        Task<IEnumerable<UserPaymentMethods_DTOS>> FILTRAR_USERPAYMENTMETHODS_ASYNC(string searchTerm, bool? statusId);

        Task<(int code, string message, int? templateId)> NUEVO_USERPAYMENTMETHODS_ASYNC(
            int userPaymentMethodUserId,
            int userPaymentMethodPaymentMethodTypeId,
            string cardNumberPlain,
            string expirationDatePlain,
            string cvvPlain,
            string userPaymentMethodCardHolderName,
            int userPaymentMethodCreatorId,
            bool userPaymentMethodStatusId
        );

        Task<(int code, string message, int? templateId)> ACTUALIZAR_USERPAYMENTMETHODS_ASYNC(
            int userPaymentMethodId,
            int userPaymentMethodPaymentMethodTypeId,
            string cardNumberPlain,
            string expirationDatePlain,
            string cvvPlain,
            string userPaymentMethodCardHolderName,
            int userPaymentMethodModificatorId,
            bool userPaymentMethodStatusId,
            bool forzarRecuperacion
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_USERPAYMENTMETHODS_ASYNC(
            int userPaymentMethodId,
            int userPaymentMethodModificatorId
        );
    }
}
