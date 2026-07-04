using Ecom_Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom_Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users_Controller : ControllerBase
    {
        private readonly User_Services _service;

        public Users_Controller(User_Services service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _service.LISTAR_USER_ASYNC();

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
        public async Task<IActionResult> Filtrar(string searchTerm)
        {
            try
            {
                var result = await _service.FILTRAR_USER_ASYNC(searchTerm);

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
        public async Task<IActionResult> Nuevo(
            string userFullName,
            string userName,
            string userPasswordPlain,
            string userEmail,
            string userPhoneNumber,
            int userCountryId,
            int userGenderId,
            DateTime userBirthDay,
            int userCreatorId,
            int userStatusId)
        {
            try
            {
                var result = await _service.NUEVO_USER_ASYNC(
                    userFullName,
                    userName,
                    userPasswordPlain,
                    userEmail,
                    userPhoneNumber,
                    userCountryId,
                    userGenderId,
                    userBirthDay,
                    userCreatorId,
                    userStatusId);

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
        public async Task<IActionResult> Actualizar(
            int userId,
            string userFullName,
            string userEmail,
            string userPhoneNumber,
            int userModificatorId,
            int userStatusId,
            bool forzarRecuperacion)
        {
            try
            {
                var result = await _service.ACTUALIZAR_USER_ASYNC(
                    userId,
                    userFullName,
                    userEmail,
                    userPhoneNumber,
                    userModificatorId,
                    userStatusId,
                    forzarRecuperacion);

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

        [HttpDelete("Eliminar/{userId}/{userModificatorId}")]
        public async Task<IActionResult> Eliminar(int userId, int userModificatorId)
        {
            try
            {
                var result = await _service.ELIMINAR_USER_ASYNC(userId, userModificatorId);

                return Ok(new
                {
                    result.code,
                    result.message
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