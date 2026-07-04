using Ecom_Aplication.Dtos;
using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeProductVariables_Controller : ControllerBase
    {
        private readonly AttributeProductVariables_Services _service;

        public AttributeProductVariables_Controller(AttributeProductVariables_Services service)
        {
            _service = service;
        }

        [HttpPost("Nuevo")]
        public async Task<IActionResult> Nuevo([FromBody] AttributeProductVariables_DTOS dto)
        {
            try
            {
                var result = await _service.NUEVO_ATTRIBUTEPRODUCTVARIABLES_ASYNC(dto);

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
        public async Task<IActionResult> Actualizar([FromBody] AttributeProductVariables_DTOS dto)
        {
            try
            {
                var result = await _service.ACTUALIZAR_ATTRIBUTEPRODUCTVARIABLES_ASYNC(dto);

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

        [HttpGet("ObtenerPorProductVariable/{productVariableId}")]
        public async Task<IActionResult> ObtenerPorProductVariable(int productVariableId)
        {
            try
            {
                var result = await _service.OBTENER_POR_PRODUCTVARIABLE(productVariableId);

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

        [HttpDelete("Eliminar/{attributeProductVariableId}/{attributeProductVariableModificatorId}")]
        public async Task<IActionResult> Eliminar(int attributeProductVariableId, int attributeProductVariableModificatorId)
        {
            try
            {
                var result = await _service.Eliminar_AttributeProductVariables(
                    attributeProductVariableId,
                    attributeProductVariableModificatorId);

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