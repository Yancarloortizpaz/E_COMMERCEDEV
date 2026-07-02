using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class PaymentMethodTypes_Services : IPaymentMethodTypes
    {
        private readonly IPaymentMethodTypes _paymentMethodTypesRepository;

        public PaymentMethodTypes_Services(IPaymentMethodTypes paymentMethodTypesRepository)
        {
            _paymentMethodTypesRepository = paymentMethodTypesRepository;
        }

        public async Task<IEnumerable<PaymentMethodTypes_DTOS>> LISTAR_PAYMENTMETHODTYPES_ASYNC()
        {
            return await _paymentMethodTypesRepository.LISTAR_PAYMENTMETHODTYPES_ASYNC();
        }

        public async Task<IEnumerable<PaymentMethodTypes_DTOS>> FILTRAR_PAYMENTMETHODTYPES_ASYNC(string searchTerm, bool? statusId)
        {
            return await _paymentMethodTypesRepository.FILTRAR_PAYMENTMETHODTYPES_ASYNC(searchTerm, statusId);
        }

        public async Task<(int code, string message)> NUEVO_PAYMENTMETHODTYPES_ASYNC(
            string paymentMethodTypeName,
            string paymentMethodTypeDescription,
            int paymentMethodTypeCreatorId,
            bool paymentMethodTypeStatusId)
        {
            return await _paymentMethodTypesRepository.NUEVO_PAYMENTMETHODTYPES_ASYNC(
                paymentMethodTypeName,
                paymentMethodTypeDescription,
                paymentMethodTypeCreatorId,
                paymentMethodTypeStatusId
            );
        }

        public async Task<(int code, string message)> ACTUALIZAR_PAYMENTMETHODTYPES_ASYNC(
            int paymentMethodTypeId,
            string paymentMethodTypeName,
            string paymentMethodTypeDescription,
            int paymentMethodTypeModificatorId,
            bool paymentMethodTypeStatusId)
        {
            return await _paymentMethodTypesRepository.ACTUALIZAR_PAYMENTMETHODTYPES_ASYNC(
                paymentMethodTypeId,
                paymentMethodTypeName,
                paymentMethodTypeDescription,
                paymentMethodTypeModificatorId,
                paymentMethodTypeStatusId
            );
        }

        public async Task<(int code, string message)> ELIMINAR_PAYMENTMETHODTYPES_ASYNC(
            int paymentMethodTypeId,
            int paymentMethodTypeModificatorId)
        {
            return await _paymentMethodTypesRepository.ELIMINAR_PAYMENTMETHODTYPES_ASYNC(paymentMethodTypeId, paymentMethodTypeModificatorId);
        }
    }
}