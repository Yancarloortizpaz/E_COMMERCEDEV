using Ecom_Aplication.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface IPaymentOrders
    {
        
        Task<IEnumerable<PaymentOrders_DTOS>> LISTAR_PAYMENTORDERS_ASYNC();

       
        Task<IEnumerable<PaymentOrders_DTOS>> FILTRAR_PAYMENTORDERS_ASYNC(int? userId, string searchTerm, int? statusId);

     
        Task<(int code, string message, int? templateId)> NUEVO_PAYMENTORDERS_ASYNC(
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
            int orderStatusId
        );
    }
}