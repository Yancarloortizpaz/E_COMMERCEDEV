using Ecom_Domain;
using Ecom_Infraestructure.Database;
using Microsoft.Data.SqlClient;
using modu.application.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ecom_Infraestructure.Repository
{
    public class User_Repository : IUserRepository
    {
        private readonly DB_conection _conection;

        public User_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        
        public async Task<IEnumerable<User>> LISTAR_USER_ASYNC()
        {
            var list = new List<User>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_SECURITY].[sp_Users_List]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(MapearEntidadDominio(dr));
                        }
                    }
                }
                return list;
            }
            catch (SqlException) { throw; }
        }

        
        public async Task<IEnumerable<User>> FILTRAR_USER_ASYNC(string searchTerm)
        {
            var list = new List<User>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_SECURITY].[sp_Users_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@searchTerm", searchTerm ?? (object)DBNull.Value));

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(MapearEntidadDominio(dr));
                        }
                    }
                }
                return list;
            }
            catch (SqlException) { throw; }
        }

        
        public async Task<(int code, string message, int? templateId)> NUEVO_USER_ASYNC(
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
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_SECURITY].[sp_Users_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userFullName", userFullName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userName", userName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userPasswordPlain", userPasswordPlain ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userEmail", userEmail ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userPhoneNumber", userPhoneNumber ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userCountryId", userCountryId));
                    cmd.Parameters.Add(new SqlParameter("@userGenderId", userGenderId));
                    cmd.Parameters.Add(new SqlParameter("@userBirthDay", userBirthDay));
                    cmd.Parameters.Add(new SqlParameter("@userCreatorId", userCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@userStatusId", userStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Usuario creado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el usuario", ex); }
        }

        // Actualiza los datos del usuario. Si está inactivo, pide forzarRecuperacion = true
        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_USER_ASYNC(
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
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_SECURITY].[sp_Users_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userId", userId));
                    cmd.Parameters.Add(new SqlParameter("@userFullName", userFullName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userEmail", userEmail ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userPhoneNumber", userPhoneNumber ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userModificatorId", userModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@userStatusId", userStatusId));
                    cmd.Parameters.Add(new SqlParameter("@ForzarRecuperacion", forzarRecuperacion));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Usuario actualizado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el usuario", ex); }
        }

        // Borrado lógico del usuario (Cambia su estado a inactivo/eliminado)
        public async Task<(int code, string message)> ELIMINAR_USER_ASYNC(int userId, int userModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_SECURITY].[sp_Users_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userId", userId));
                    cmd.Parameters.Add(new SqlParameter("@userModificatorId", userModificatorId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Usuario eliminado exitosamente.";

                    return (code, message);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el usuario", ex); }
        }

        private User MapearEntidadDominio(SqlDataReader dr)
        {
            return new User
            {
                UserId = dr["userId"] as int?,
                UserFullName = dr["userFullName"]?.ToString() ?? string.Empty,
                UserName = dr["userName"]?.ToString() ?? string.Empty,
                UserPassword = dr["UserPassword"] as byte[],
                UserEmail = dr["userEmail"]?.ToString() ?? string.Empty,
                UserPhoneNumber = dr["userPhoneNumber"]?.ToString() ?? string.Empty,
                UserCountryId = dr["userCountryId"] as int?,
                UserGenderId = dr["userGenderId"] as int?,
                UserBirthDay = dr["userBirthDay"] as DateTime?,
                UserCreatorId = dr["userCreatorId"] as int?,
                UserCreationDate = dr["userCreationDate"] as DateTime?,
                UserModificatorId = dr["userModificatorId"] as int?,
                UserModificationDate = dr["userModificationDate"] as DateTime?,
                UserStatusId = dr["userStatusId"] as int?
            };
        }
    }
}