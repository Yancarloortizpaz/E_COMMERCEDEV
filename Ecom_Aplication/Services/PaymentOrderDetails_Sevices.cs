using Ecom_Aplication.Dtos;
using Ecom_Domain;
using modu.application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class PaymentOrderDetails_Services : IPaymentOrderDetails
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

        public async Task<IEnumerable<PaymentOrderDetails_DTOS>> FILTRAR_PAYMENTORDERDETAILS_ASYNC(int? orderId, string searchTerm)
        {
            return await _paymentOrderDetails.FILTRAR_PAYMENTORDERDETAILS_ASYNC(orderId, searchTerm);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_PAYMENTORDERDETAILS_ASYNC(
            int orderDetailOrderId,
            int orderDetailProductVariableId,
            decimal orderDetailPrice,
            int orderDetailQuantity,
            decimal orderDetailDiscount,
            decimal orderDetailSubTotal,
            decimal orderDetailTAX,
            decimal orderDetailTotal,
            int orderDetailCurrencyId,
            int orderDetailCreatorId,
            bool orderDetailStatusId)
        {
            return await _paymentOrderDetails.NUEVO_PAYMENTORDERDETAILS_ASYNC(
                orderDetailOrderId,
                orderDetailProductVariableId,
                orderDetailPrice,
                orderDetailQuantity,
                orderDetailDiscount,
                orderDetailSubTotal,
                orderDetailTAX,
                orderDetailTotal,
                orderDetailCurrencyId,
                orderDetailCreatorId,
                orderDetailStatusId
            );
        }
    }
}