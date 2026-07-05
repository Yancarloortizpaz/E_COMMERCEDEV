using Ecom_Aplication.Dtos;
using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributesTypes_Controller : ControllerBase
    {
        private readonly AttributesType_Services _service;

        public AttributesTypes_Controller(AttributesType_Services service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _service.LISTAR_ATTRIBUTESTYPES_ASYNC();

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

        [HttpGet("Obtener/{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            try
            {
                var result = await _service.OBTENER_ATTRIBUTESTYPE_BY_ID_ASYNC(id);

                if (result == null)
                {
                    return NotFound(new
                    {
                        code = 404,
                        message = "Registro no encontrado."
                    });
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
                return StatusCode(500, new
                {
                    code = 500,
                    message = ex.Message
                });
            }
        }

        [HttpPost("Nuevo")]
        public async Task<IActionResult> Nuevo([FromBody] AttributesTypes_DTOS dto)
        {
            try
            {
                var (code, message, templateId) = await _service.NUEVO_ATTRIBUTESTYPE_ASYNC(dto);

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

        [HttpPut("Actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] AttributesTypes_DTOS dto)
        {
            try
            {
                var (code, message, templateId) = await _service.ACTUALIZAR_ATTRIBUTESTYPE_ASYNC(dto);

                if (code != 200)
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

        [HttpDelete("Eliminar")]
        public async Task<IActionResult> Eliminar([FromBody] AttributesTypes_DTOS dto)
        {
            try
            {
                // Cambiado a FromBody DTO completo porque tu base de datos exige validar 
                // todas las propiedades del objeto original antes de realizar el borrado lógico.
                var (code, message, templateId) = await _service.ELIMINAR_ATTRIBUTESTYPE_ASYNC(dto);

                if (code != 200)
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