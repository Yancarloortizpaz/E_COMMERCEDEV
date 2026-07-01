using Ecom_Aplication.Interfaces;
using Ecom_Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom_Aplication.Services
{
    public class CartDetail_Services : ICartDetails
    {
        private readonly ICartDetails _cartDetailsRepository;

        public CartDetail_Services(ICartDetails cartDetailsRepository)
        {
            _cartDetailsRepository = cartDetailsRepository;
        }

        public async Task<IEnumerable<CartDetail>> LISTAR_CARTDETAILS_ASYNC()
        {
            return await _cartDetailsRepository.LISTAR_CARTDETAILS_ASYNC();
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_CARTDETAILS_ASYNC(
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
            bool cartDetailStatusId)
        {
            return await _cartDetailsRepository.NUEVO_CARTDETAILS_ASYNC(
                cartDetailCartId,
                cartDetailProductVariableId,
                cartDetailPrice,
                cartDetailQuantity,
                cartDetailDiscount,
                cartDetailSubTotal,
                cartDetailTAX,
                cartDetailTotal,
                cartDetailCurrencyId,
                cartDetailCreatorId,
                cartDetailStatusId
            );
        }

        public async Task<(int code, string message)> ACTUALIZAR_CANTIDAD_CARTDETAILS_ASYNC(
            int cartDetailId,
            int newQuantity,
            int modificatorId)
        {
            return await _cartDetailsRepository.ACTUALIZAR_CANTIDAD_CARTDETAILS_ASYNC(
                cartDetailId,
                newQuantity,
                modificatorId
            );
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_CARTDETAILS_ASYNC(
            int cartDetailId,
            int cartDetailModificatorId)
        {
            return await _cartDetailsRepository.ELIMINAR_CARTDETAILS_ASYNC(cartDetailId, cartDetailModificatorId);
        }
    }
}