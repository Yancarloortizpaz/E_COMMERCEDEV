using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovementTypes_Controller : ControllerBase
    {
        private readonly StockMovementTypes_Services _services;

        public StockMovementTypes_Controller(StockMovementTypes_Services services)
        {
            _services = services;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _services.LISTAR_STOCKMOVEMENTTYPES_ASYNC();

                return Ok(new
                {
                    code = 200,
                    message = "Consulta realizada correctamente.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = ex.Message });
            }
        }

        [HttpGet("Obtener/{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            try
            {
                var result = await _services.OBTENER_STOCKMOVEMENTTYPE_BY_ID_ASYNC(id);

                if (result == null)
                {
                    return NotFound(new { code = 404, message = "Registro no encontrado." });
                }

                return Ok(new
                {
                    code = 200,
                    message = "Consulta realizada correctamente.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = ex.Message });
            }
        }
    }
}