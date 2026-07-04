using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentOrderDetails_Controller : ControllerBase
    {
        private readonly PaymentOrderDetails_Sevices _service;

        public PaymentOrderDetails_Controller(PaymentOrderDetails_Sevices service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _service.LISTAR_PAYMENTORDERDETAILS_ASYNC();

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
        public async Task<IActionResult> Filtrar(int paymentOrderId)
        {
            try
            {
                var result = await _service.FILTRAR_PAYMENTORDERDETAILS_ASYNC(paymentOrderId);

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
            int paymentOrderId,
            int productVariableId,
            decimal price,
            int quantity,
            decimal discount,
            decimal subtotal,
            decimal tax,
            decimal total,
            int creatorId,
            int statusId)
        {
            try
            {
                var result = await _service.NUEVO_PAYMENTORDERDETAILS_ASYNC(
                    paymentOrderId,
                    productVariableId,
                    price,
                    quantity,
                    discount,
                    subtotal,
                    tax,
                    total,
                    creatorId,
                    statusId);

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

        [HttpPut("Actualizar")]
        public async Task<IActionResult> Actualizar(
            int paymentOrderDetailId,
            int quantity,
            decimal discount,
            int modificatorId)
        {
            try
            {
                var result = await _service.ACTUALIZAR_PAYMENTORDERDETAILS_ASYNC(
                    paymentOrderDetailId,
                    quantity,
                    discount,
                    modificatorId);

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

        [HttpDelete("Eliminar/{paymentOrderDetailId}/{modificatorId}")]
        public async Task<IActionResult> Eliminar(int paymentOrderDetailId, int modificatorId)
        {
            try
            {
                var result = await _service.ELIMINAR_PAYMENTORDERDETAILS_ASYNC(
                    paymentOrderDetailId,
                    modificatorId);

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