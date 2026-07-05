using Ecom_Aplication.Dtos;
using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariables_Controller : ControllerBase
    {
        private readonly ProductVariables_Services _service;

        public ProductVariables_Controller(ProductVariables_Services service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _service.LISTAR_PRODUCTVARIABLES_ASYNC();

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

        [HttpGet("Obtener/{productVariableId}")]
        public async Task<IActionResult> Obtener(int productVariableId)
        {
            try
            {
                var result = await _service.OBTENER_PRODUCTVARIABLE_BY_ID_ASYNC(productVariableId);

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

        [HttpGet("Filtrar")]
        public async Task<IActionResult> Filtrar(string searchTerm, bool? statusId)
        {
            try
            {
                var result = await _service.FILTRAR_PRODUCTVARIABLES_ASYNC(searchTerm, statusId);

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
        public async Task<IActionResult> Nuevo([FromBody] ProductVariables_DTOS dto)
        {
            try
            {
                var (code, message, templateId) = await _service.NUEVO_PRODUCTVARIABLES_ASYNC(dto);

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
        public async Task<IActionResult> Actualizar([FromBody] ProductVariables_DTOS dto)
        {
            try
            {
                var (code, message, templateId) = await _service.ACTUALIZAR_PRODUCTVARIABLES_ASYNC(dto);

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

        [HttpDelete("Eliminar/{productVariableId}/{modificatorId}")]
        public async Task<IActionResult> Eliminar(int productVariableId, int modificatorId)
        {
            try
            {
                var (code, message, templateId) = await _service.ELIMINAR_PRODUCTVARIABLES_ASYNC(productVariableId, modificatorId);

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