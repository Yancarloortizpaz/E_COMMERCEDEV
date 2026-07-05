using Ecom_Aplication.Dtos;
using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovements_Controller : ControllerBase
    {
        private readonly StockMovements_Services _stockService;

        public StockMovements_Controller(StockMovements_Services service)
        {
            _stockService = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _stockService.LISTAR_STOCKMOVEMENTS_ASYNC();

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
                // Agregamos ?? "" para evitar que el buscador truene si envían null
                var result = await _stockService.FILTRAR_STOCKMOVEMENTS_ASYNC(searchTerm ?? "");

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
        public async Task<IActionResult> Nuevo([FromBody] StockMovements_DTOS dto)
        {
            try
            {
                // Ahora usamos el DTO limpio y directo
                var (code, message, templateId) = await _stockService.NUEVO_STOCKMOVEMENTS_ASYNC(dto);

                if (code != 200 && code != 201)
                {
                    return StatusCode(code, new
                    {
                        code,
                        message
                    });
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
                return StatusCode(500, new
                {
                    code = 500,
                    message = ex.Message
                });
            }
        }
    }
}