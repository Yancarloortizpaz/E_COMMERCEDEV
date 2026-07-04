using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovements_Controller : ControllerBase
    {
        private readonly StockMovements_Services _service;

        public StockMovements_Controller(StockMovements_Services service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _service.LISTAR_STOCKMOVEMENTS_ASYNC();

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
        public async Task<IActionResult> Filtrar(string searchTerm)
        {
            try
            {
                var result = await _service.FILTRAR_STOCKMOVEMENTS_ASYNC(searchTerm);

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
            int stockMovementType,
            int? stockMovementOrderId,
            string stockMovementReference,
            DateTime stockMovementDate,
            int stockMovementCreatorId,
            int stockMovementStatusId)
        {
            try
            {
                var result = await _service.NUEVO_STOCKMOVEMENTS_ASYNC(
                    stockMovementType,
                    stockMovementOrderId,
                    stockMovementReference,
                    stockMovementDate,
                    stockMovementCreatorId,
                    stockMovementStatusId);

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