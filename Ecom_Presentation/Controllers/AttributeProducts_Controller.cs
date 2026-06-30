using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeProducts_Controller : ControllerBase
    {
        private readonly AttributeProducts_Services _service;

        public AttributeProducts_Controller(AttributeProducts_Services service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> LISTAR_ATTRIBUTEPRODUCTS()
        {
            try
            {
                var lista = await _service.LISTAR_ATTRIBUTEPRODUCTS();
                if (lista == null || !lista.Any())
                {
                    return NotFound(new { codigo = 404, msj = "No se encontraron AttributeProducts." });
                }
                return Ok(new { codigo = 200, msj = "Consulta exitosa", data = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { codigo = 500, msj = ex.Message });
            }
        }
    }
}
