using Ecom_Aplication.Dtos;
using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImages_Controller : ControllerBase
    {
        private readonly ProductImages_Services _service;

        public ProductImages_Controller(ProductImages_Services service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _service.LISTAR_PRODUCTIMAGES_ASYNC();

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

        [HttpGet("Obtener/{productImageId}")]
        public async Task<IActionResult> Obtener(int productImageId)
        {
            try
            {
                var result = await _service.OBTENER_PRODUCTIMAGE_BY_ID_ASYNC(productImageId);

                if (result == null)
                {
                    return NotFound(new
                    {
                        code = 404,
                        message = "Registro no encontrado."
                    });
                }

                return Ok(result);
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
        public async Task<IActionResult> Nuevo([FromBody] ProductImages_DTOS dto)
        {
            try
            {
                var result = await _service.NUEVO_PRODUCTIMAGES_ASYNC(dto);

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
        public async Task<IActionResult> Actualizar([FromBody] ProductImages_DTOS dto)
        {
            try
            {
                var result = await _service.ACTUALIZAR_PRODUCTIMAGES_ASYNC(dto);

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

        [HttpDelete("Eliminar/{productImageId}/{modificatorId}")]
        public async Task<IActionResult> Eliminar(int productImageId, int modificatorId)
        {
            try
            {
                var result = await _service.ELIMINAR_PRODUCTIMAGES_ASYNC(productImageId, modificatorId);

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