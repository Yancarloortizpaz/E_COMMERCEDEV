using Ecom_Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Interfaces
{
    public interface ICartDetails
    {
        Task<IEnumerable<CartDetail>> LISTAR_CARTDETAILS_ASYNC();

        Task<(int code, string message, int? templateId)> NUEVO_CARTDETAILS_ASYNC(
            int cartDetailCartId,
            int cartDetailProductVariableId,
            decimal cartDetailPrice,
            int cartDetailQuantity,
            decimal cartDetailDiscount,
            decimal cartDetailSubTotal,
            decimal cartDetailTAX,
            decimal cartDetailTotal,
            int cartDetailCurrencyId,
            int cartDetailCreatorId,
            bool cartDetailStatusId
        );

        Task<(int code, string message)> ACTUALIZAR_CANTIDAD_CARTDETAILS_ASYNC(
            int cartDetailId,
            int newQuantity,
            int modificatorId
        );

        Task<(int code, string message, int? templateId)> ELIMINAR_CARTDETAILS_ASYNC(
            int cartDetailId,
            int cartDetailModificatorId
        );
    }
}