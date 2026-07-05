using Ecom_Aplication.Dtos;
using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovementDetails_Controller : ControllerBase
    {
        private readonly StockMovementDetails_Services _detailsService;

        public StockMovementDetails_Controller(StockMovementDetails_Services service)
        {
            _detailsService = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _detailsService.LISTAR_STOCKMOVEMENTDETAILS_ASYNC();

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

        [HttpGet("Filtrar")]

        public async Task<IActionResult> Filtrar(int? movementId, string searchTerm)
        {
            try
            {

                var result = await _detailsService.FILTRAR_STOCKMOVEMENTDETAILS_ASYNC(movementId, searchTerm ?? "");

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

        [HttpPost("Nuevo")]
        public async Task<IActionResult> Nuevo([FromBody] StockMovementDetails_DTOS dto)
        {
            try
            {
                var (code, message, templateId) = await _detailsService.NUEVO_STOCKMOVEMENTDETAILS_ASYNC(dto);

                if (code != 200 && code != 201)
                {
                    return StatusCode(code, new { code, message });
                }

                return Ok(new
                {
                    code,
                    message,
                    templateId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = ex.Message });
            }
        }
    }
}