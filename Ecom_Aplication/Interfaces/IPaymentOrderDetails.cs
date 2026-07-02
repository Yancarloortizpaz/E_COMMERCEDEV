using Ecom_Aplication.Dtos;
using Ecom_Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace modu.application.Interface
{
    public interface IPaymentOrderDetails
    {
        
        Task<IEnumerable<PaymentOrderDetails_DTOS>> LISTAR_PAYMENTORDERDETAILS_ASYNC();

       
        Task<IEnumerable<PaymentOrderDetails_DTOS>> FILTRAR_PAYMENTORDERDETAILS_ASYNC(int? orderId, string searchTerm);

        
        Task<(int code, string message, int? templateId)> NUEVO_PAYMENTORDERDETAILS_ASYNC(
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
            bool orderDetailStatusId
        );
    }
}