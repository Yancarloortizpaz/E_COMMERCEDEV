using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartDetails_Controller : ControllerBase
    {
        private readonly CartDetail_Services _service;

        public CartDetails_Controller(CartDetail_Services service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _service.LISTAR_CARTDETAILS_ASYNC();

                return Ok(new
                {
                    code = 200,
                    message = "Consulta realizada correctamente.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = ex.Message
                });
            }
        }

        [HttpPost("Nuevo")]
        public async Task<IActionResult> Nuevo(
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
            try
            {
                var result = await _service.NUEVO_CARTDETAILS_ASYNC(
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
                    cartDetailStatusId);

                return Ok(new
                {
                    result.code,
                    result.message,
                    result.templateId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = ex.Message
                });
            }
        }

        [HttpPut("ActualizarCantidad")]
        public async Task<IActionResult> ActualizarCantidad(
            int cartDetailId,
            int newQuantity,
            int modificatorId)
        {
            try
            {
                var result = await _service.ACTUALIZAR_CANTIDAD_CARTDETAILS_ASYNC(
                    cartDetailId,
                    newQuantity,
                    modificatorId);

                return Ok(new
                {
                    result.code,
                    result.message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = ex.Message
                });
            }
        }

        [HttpDelete("Eliminar/{cartDetailId}/{cartDetailModificatorId}")]
        public async Task<IActionResult> Eliminar(
            int cartDetailId,
            int cartDetailModificatorId)
        {
            try
            {
                var result = await _service.ELIMINAR_CARTDETAILS_ASYNC(
                    cartDetailId,
                    cartDetailModificatorId);

                return Ok(new
                {
                    result.code,
                    result.message,
                    result.templateId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = ex.Message
                });
            }
        }
    }
}