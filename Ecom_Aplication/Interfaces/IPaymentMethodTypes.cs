using Ecom_Aplication.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IPaymentMethodTypes
    {
        
        Task<IEnumerable<PaymentMethodTypes_DTOS>> LISTAR_PAYMENTMETHODTYPES_ASYNC();

        
        Task<IEnumerable<PaymentMethodTypes_DTOS>> FILTRAR_PAYMENTMETHODTYPES_ASYNC(string searchTerm, bool? statusId);

        
        Task<(int code, string message)> NUEVO_PAYMENTMETHODTYPES_ASYNC(
            string paymentMethodTypeName,
            string paymentMethodTypeDescription,
            int paymentMethodTypeCreatorId,
            bool paymentMethodTypeStatusId
        );

        
        Task<(int code, string message)> ACTUALIZAR_PAYMENTMETHODTYPES_ASYNC(
            int paymentMethodTypeId,
            string paymentMethodTypeName,
            string paymentMethodTypeDescription,
            int paymentMethodTypeModificatorId,
            bool paymentMethodTypeStatusId
        );

      
        Task<(int code, string message)> ELIMINAR_PAYMENTMETHODTYPES_ASYNC(
            int paymentMethodTypeId,
            int paymentMethodTypeModificatorId
        );
    }
}
