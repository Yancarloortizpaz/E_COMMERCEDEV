using Ecom_Aplication.Dtos;
using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishLists_Controller : ControllerBase
    {
        private readonly WishLists_Services _service;

        public WishLists_Controller(WishLists_Services service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _service.LISTAR_WISHLISTS_ASYNC();

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

        [HttpGet("Obtener/{wishListId}")]
        public async Task<IActionResult> Obtener(int wishListId)
        {
            try
            {
                var result = await _service.OBTENER_WISHLIST_BY_ID_ASYNC(wishListId);

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

        [HttpGet("Filtrar")]
        public async Task<IActionResult> Filtrar(int userId)
        {
            try
            {
                var result = await _service.FILTRAR_WISHLISTS_ASYNC(userId);

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
        public async Task<IActionResult> Nuevo([FromBody] WishLists_DTOS dto)
        {
            try
            {
                var result = await _service.NUEVO_WISHLISTS_ASYNC(dto);

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
        public async Task<IActionResult> Actualizar([FromBody] WishLists_DTOS dto)
        {
            try
            {
                var result = await _service.ACTUALIZAR_WISHLISTS_ASYNC(dto);

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

        [HttpDelete("Eliminar/{wishListId}/{modificatorId}")]
        public async Task<IActionResult> Eliminar(int wishListId, int modificatorId)
        {
            try
            {
                var result = await _service.ELIMINAR_WISHLISTS_ASYNC(wishListId, modificatorId);

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