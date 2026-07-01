using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class PaymentOrders_Services : IPaymentOrders
    {
        private readonly IPaymentOrders _paymentOrdersRepository;

        public PaymentOrders_Services(IPaymentOrders paymentOrdersRepository)
        {
            _paymentOrdersRepository = paymentOrdersRepository;
        }

        public async Task<IEnumerable<PaymentOrders_DTOS>> LISTAR_PAYMENTORDERS_ASYNC()
        {
            return await _paymentOrdersRepository.LISTAR_PAYMENTORDERS_ASYNC();
        }

        public async Task<IEnumerable<PaymentOrders_DTOS>> FILTRAR_PAYMENTORDERS_ASYNC(int? userId, string searchTerm, int? statusId)
        {
            return await _paymentOrdersRepository.FILTRAR_PAYMENTORDERS_ASYNC(userId, searchTerm, statusId);
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_PAYMENTORDERS_ASYNC(
            int orderUserId,
            int orderDeliveryAddress,
            int orderPaymentMethodId,
            decimal? orderSubtotal,
            decimal? orderDiscount,
            decimal orderShipping,
            decimal? orderTAX,
            decimal? orderTotal,
            int? orderCurrencyId,
            int orderCreatorId,
            int orderStatusId)
        {
            return await _paymentOrdersRepository.NUEVO_PAYMENTORDERS_ASYNC(
                orderUserId,
                orderDeliveryAddress,
                orderPaymentMethodId,
                orderSubtotal,
                orderDiscount,
                orderShipping,
                orderTAX,
                orderTotal,
                orderCurrencyId,
                orderCreatorId,
                orderStatusId
            );
        }
    }
}