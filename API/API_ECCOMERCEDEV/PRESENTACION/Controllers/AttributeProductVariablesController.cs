using APLICATION.DTOs.AttributeProductVariables;
using APLICATION.Services;
using DOMAIN.VariablesSalida;
using Microsoft.AspNetCore.Mvc;

namespace PRESENTACION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeProductVariablesController : ControllerBase
    {
        private readonly AttributeProductVariablesServices _service;

        public AttributeProductVariablesController(AttributeProductVariablesServices service)
        {
            _service = service;
        }

        #region lectura_attributeproductvariables

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar_AttributeProductVariables()
        {
            try
            {
                var lista = await _service.Listar_AttributeProductVariables_async();
                if (lista == null || !lista.Any())
                {
                    return NotFound(new { codigo = 404, msj = "No se encontraron variables de atributos de productos." });
                }
                return Ok(new { codigo = 200, msj = "Consulta exitosa", data = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { codigo = 500, msj = ex.Message });
            }
        }

        [HttpGet("filtrar")]
        public async Task<IActionResult> Filtrar_AttributeProductVariables([FromQuery] AttributeProductVariablesFilterDTOs filtro)
        {
            try
            {
                var lista = await _service.Filtrar_AttributeProductVariables_async(filtro);
                if (lista == null || !lista.Any())
                {
                    return NotFound(new { codigo = 404, msj = "No se encontraron variables de atributos de productos que coincidan con la búsqueda." });
                }
                return Ok(new { codigo = 200, msj = "Consulta exitosa", data = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { codigo = 500, msj = ex.Message });
            }
        }

        #endregion

        #region escritura_attributeproductvariables

        [HttpPost("insertar")]
        public async Task<IActionResult> Ingresar_AttributeProductVariables([FromBody] AttributeProductVariablesInsertarDTOs item)
        {
            try
            {
                if (!ModelState.IsValid || item == null)
                {
                    return BadRequest(new { codigo = 400, msj = "Datos enviados no válidos." });
                }

                OUTPUT resultado = await _service.Insertar_AttributeProductVariables_async(item);

                if (!resultado.IsSuccess)
                {
                    return BadRequest(new { codigo = resultado.Code, msj = resultado.Message });
                }

                return Ok(new { codigo = resultado.Code, msj = resultado.Message, templateId = resultado.TemplateId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { codigo = 500, msj = ex.Message });
            }
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Editar_AttributeProductVariables([FromBody] AttributeProductVariablesEditarDTOs item)
        {
            try
            {
                if (item == null || !item.AttributeProductVariableId.HasValue)
                {
                    return BadRequest(new { codigo = 400, msj = "El identificador de la variable de atributo de producto es obligatorio." });
                }

                OUTPUT resultado = await _service.Editar_AttributeProductVariables_async(item);

                if (!resultado.IsSuccess)
                {
                    return BadRequest(new { codigo = resultado.Code, msj = resultado.Message });
                }

                return Ok(new { codigo = resultado.Code, msj = resultado.Message, templateId = resultado.TemplateId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { codigo = 500, msj = ex.Message });
            }
        }

        [HttpDelete("{id}/{idModificador}")]
        public async Task<IActionResult> Eliminar_AttributeProductVariables(int? id, int? idModificador)
        {
            try
            {
                if (!id.HasValue || !idModificador.HasValue)
                {
                    return BadRequest(new { codigo = 400, msj = "El ID de la variable de atributo de producto y el ID del modificador son requeridos." });
                }

                OUTPUT resultado = await _service.Eliminar_AttributeProductVariables_async(id, idModificador);

                if (!resultado.IsSuccess)
                {
                    return BadRequest(new { codigo = resultado.Code, msj = resultado.Message });
                }

                return Ok(new { codigo = resultado.Code, msj = resultado.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { codigo = 500, msj = ex.Message });
            }
        }

        #endregion
    }
}
