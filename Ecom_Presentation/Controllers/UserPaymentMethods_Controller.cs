using Ecom_Aplication.Dtos;
using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPaymentMethods_Controller : ControllerBase
    {
        private readonly UserPaymentMethods_Services _services;

        public UserPaymentMethods_Controller(UserPaymentMethods_Services services)
        {
            _services = services;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _services.LISTAR_USERPAYMENTMETHODS_ASYNC();

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
 
                var lista = await _services.OBTENER_USERPAYMENTMETHOD_BY_ID_ASYNC(id);
                var result = lista.FirstOrDefault();

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

        [HttpGet("Filtrar")]
        public async Task<IActionResult> Filtrar(string searchTerm, bool? statusId)
        {
            try
            {
                var result = await _services.FILTRAR_USERPAYMENTMETHODS_ASYNC(searchTerm ?? "", statusId);

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
        public async Task<IActionResult> Nuevo([FromBody] UserPaymentMethods_DTOS dto)
        {
            try
            {
                var (code, message, templateId) = await _services.NUEVO_USERPAYMENTMETHODS_ASYNC(dto);

                if (code != 200 && code != 201)
                {
                    return StatusCode(code, new { code, message });
                }

                return Ok(new { code, message, templateId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = ex.Message });
            }
        }

        [HttpPut("Actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] UserPaymentMethods_DTOS dto)
        {
            try
            {
                var (code, message, templateId) = await _services.ACTUALIZAR_USERPAYMENTMETHODS_ASYNC(dto);

                if (code != 200)
                {
                    return StatusCode(code, new { code, message });
                }

                return Ok(new { code, message, templateId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = ex.Message });
            }
        }

        [HttpDelete("Eliminar/{id}/{modificatorId}")]
        public async Task<IActionResult> Eliminar(int id, int modificatorId)
        {
            try
            {
                var (code, message, templateId) = await _services.ELIMINAR_USERPAYMENTMETHODS_ASYNC(id, modificatorId);

                if (code != 200)
                {
                    return StatusCode(code, new { code, message });
                }

                return Ok(new { code, message, templateId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = 500, message = ex.Message });
            }
        }
    }
}