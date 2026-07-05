using Ecom_Aplication.Dtos;
using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Products_Controller : ControllerBase
    {
        private readonly Products_Services _service;

        public Products_Controller(Products_Services service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _service.LISTAR_PRODUCTS_ASYNC();

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

        [HttpGet("Obtener/{productId}")]
        public async Task<IActionResult> Obtener(int productId)
        {
            try
            {
                var result = await _service.OBTENER_PRODUCT_BY_ID_ASYNC(productId);

                if (result == null)
                {
                    return NotFound(new
                    {
                        code = 404,
                        message = "Producto no encontrado."
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
        public async Task<IActionResult> Filtrar(string searchTerm, bool? statusId)
        {
            try
            {
                var result = await _service.FILTRAR_PRODUCTS_ASYNC(searchTerm ?? "", statusId);

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
        public async Task<IActionResult> Nuevo([FromBody] Products_DTOS dto)
        {
            try
            {
                var result = await _service.NUEVO_PRODUCTS_ASYNC(dto);

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
        public async Task<IActionResult> Actualizar([FromBody] Products_DTOS dto)
        {
            try
            {
                var result = await _service.ACTUALIZAR_PRODUCTS_ASYNC(dto);

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

        [HttpDelete("Eliminar/{productId}/{modificatorId}")]
        public async Task<IActionResult> Eliminar(int productId, int modificatorId)
        {
            try
            {
                var result = await _service.ELIMINAR_PRODUCTS_ASYNC(productId, modificatorId);

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