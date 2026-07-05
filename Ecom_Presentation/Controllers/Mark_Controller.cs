using Ecom_Aplication.Dtos;
using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Mark_Controller : ControllerBase
    {
        private readonly Mark_Services _service;

        public Mark_Controller(Mark_Services service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _service.LISTAR_MARK();

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

        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string? searchTerm, [FromQuery] bool? statusId)
        {
            try
            {
                var result = await _service.Filtrar_Marks(searchTerm, statusId);

                return Ok(new
                {
                    code = 200,
                    message = "Búsqueda realizada correctamente.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = ex.Message });
            }
        }

        [HttpPost("Nuevo")]
        public async Task<IActionResult> Nuevo([FromBody] Marks_DTOS dto)
        {
            try
            {
                var result = await _service.NUEVO_MARK_ASYNC(dto);

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
        public async Task<IActionResult> Actualizar([FromBody] Marks_DTOS dto)
        {
            try
            {
                var result = await _service.ACTUALIZAR_MARK_ASYNC(dto);

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

        [HttpDelete("Eliminar/{markId}/{modificatorId}")]
        public async Task<IActionResult> Eliminar(int markId, int modificatorId)
        {
            try
            {
                var result = await _service.Eliminar_Mark(markId, modificatorId);

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