using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class PaymentMethodTypes_Services
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

        public async Task<PaymentMethodTypes_DTOS?> OBTENER_PAYMENTMETHODTYPE_BY_ID_ASYNC(int id)
        {
            var data = await _paymentMethodTypesRepository.FILTRAR_PAYMENTMETHODTYPES_ASYNC(id.ToString(), null);
            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<PaymentMethodTypes_DTOS>> FILTRAR_PAYMENTMETHODTYPES_ASYNC(string? searchTerm, bool? statusId)
        {
            return await _paymentMethodTypesRepository.FILTRAR_PAYMENTMETHODTYPES_ASYNC(searchTerm ?? "", statusId);
        }

        public async Task<(int code, string message)> NUEVO_PAYMENTMETHODTYPES_ASYNC(PaymentMethodTypes_DTOS dto)
        {
            return await _paymentMethodTypesRepository.NUEVO_PAYMENTMETHODTYPES_ASYNC(
                dto.paymentMethodTypeName ?? "",
                dto.paymentMethodTypeDescription ?? "",
                dto.paymentMethodTypeCreatorId ?? 0,
                dto.paymentMethodTypeStatusId ?? false
            );
        }

        public async Task<(int code, string message)> ACTUALIZAR_PAYMENTMETHODTYPES_ASYNC(PaymentMethodTypes_DTOS dto)
        {
            return await _paymentMethodTypesRepository.ACTUALIZAR_PAYMENTMETHODTYPES_ASYNC(
                dto.paymentMethodTypeId ?? 0,
                dto.paymentMethodTypeName ?? "",
                dto.paymentMethodTypeDescription ?? "",
                dto.paymentMethodTypeModificatorId ?? 0,
                dto.paymentMethodTypeStatusId ?? false
            );
        }

        public async Task<(int code, string message)> ELIMINAR_PAYMENTMETHODTYPES_ASYNC(int id, int modificatorId)
        {
            return await _paymentMethodTypesRepository.ELIMINAR_PAYMENTMETHODTYPES_ASYNC(id, modificatorId);
        }
    }
}