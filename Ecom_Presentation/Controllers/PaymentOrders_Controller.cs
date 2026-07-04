using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentOrders_Controller : ControllerBase
    {
        private readonly PaymentOrders_Services _service;

        public PaymentOrders_Controller(PaymentOrders_Services service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _service.LISTAR_PAYMENTORDERS_ASYNC();

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

        [HttpGet("Filtrar")]
        public async Task<IActionResult> Filtrar(int? userId, string searchTerm, int? statusId)
        {
            try
            {
                var result = await _service.FILTRAR_PAYMENTORDERS_ASYNC(userId, searchTerm, statusId);

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
            try
            {
                var result = await _service.NUEVO_PAYMENTORDERS_ASYNC(
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
                    orderStatusId);

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