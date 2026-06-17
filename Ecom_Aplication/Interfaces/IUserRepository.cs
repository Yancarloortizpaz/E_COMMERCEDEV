using Ecom_Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace modu.application.Interface
{
    public interface IUserRepository
    {
        // Trae todos los usuarios (Desencripta contraseñas con la llave simétrica)
        Task<IEnumerable<User>> LISTAR_USER_ASYNC();

        // Busca usuarios por ID, Nombre, Nombre de usuario, Email o Teléfono
        Task<IEnumerable<User>> FILTRAR_USER_ASYNC(string searchTerm);

        // Registra un nuevo usuario y valida que no se repita el correo o username
        // Retorna: code (200 OK o -1 Error), message (Validación de SQL) y templateId (ID generado)
        Task<(int code, string message, int? templateId)> NUEVO_USER_ASYNC(
            string userFullName,
            string userName,
            string userPasswordPlain,
            string userEmail,
            string userPhoneNumber,
            int userCountryId,
            int userGenderId,
            DateTime userBirthDay,
            int userCreatorId,
            int userStatusId
        );

        // Actualiza los datos del usuario. Si está inactivo, pide forzarRecuperacion = true
        // Retorna: code (200 o error), message (Detalle de SQL) y templateId (ID modificado)
        Task<(int code, string message, int? templateId)> ACTUALIZAR_USER_ASYNC(
            int userId,
            string userFullName,
            string userEmail,
            string userPhoneNumber,
            int userModificatorId,
            int userStatusId,
            bool forzarRecuperacion
        );

        // Borrado lógico del usuario (Cambia su estado a inactivo/eliminado)
        // Retorna: code (200 o error) y message (Si ya estaba eliminado o no existe)
        Task<(int code, string message)> ELIMINAR_USER_ASYNC(int userId, int userModificatorId);
    }
}