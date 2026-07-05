using Ecom_Aplication.Dtos;
using Ecom_Domain;
using modu.application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class PaymentOrderDetails_Services
    {
        private readonly IPaymentOrderDetails _paymentOrderDetails;

        public PaymentOrderDetails_Services(IPaymentOrderDetails paymentOrderDetails)
        {
            _paymentOrderDetails = paymentOrderDetails;
        }

        public async Task<IEnumerable<PaymentOrderDetails_DTOS>> LISTAR_PAYMENTORDERDETAILS_ASYNC()
        {
            return await _paymentOrderDetails.LISTAR_PAYMENTORDERDETAILS_ASYNC();
        }

        public async Task<IEnumerable<PaymentOrderDetails_DTOS>> FILTRAR_PAYMENTORDERDETAILS_ASYNC(int? orderId, string? searchTerm)
        {
            return await _paymentOrderDetails.FILTRAR_PAYMENTORDERDETAILS_ASYNC(orderId, searchTerm ?? "");
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_PAYMENTORDERDETAILS_ASYNC(PaymentOrderDetails_DTOS dto)
        {
 
            return await _paymentOrderDetails.NUEVO_PAYMENTORDERDETAILS_ASYNC(
                dto.orderDetailOrderId ?? 0,
                dto.orderDetailProductVariableId ?? 0,
                dto.orderDetailPrice ?? 0,
                dto.orderDetailQuantity ?? 0,
                dto.orderDetailDiscount ?? 0,
                dto.orderDetailSubTotal ?? 0,
                dto.orderDetailTAX ?? 0, 
                dto.orderDetailTotal ?? 0,
                dto.orderDetailCurrencyId ?? 0,
                dto.orderDetailCreatorId ?? 0,
                dto.orderDetailStatusId ?? false
            );
        }
    }
}