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
    public class UserAddress_Repository : IUserAddressRepository
    {
        private readonly DB_conection _conection;

        public UserAddress_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<UserAddress>> LISTAR_USER_ADDRESS_ASYNC()
        {
            var list = new List<UserAddress>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_UserAddress_List]", con))
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

        public async Task<IEnumerable<UserAddress>> FILTRAR_USER_ADDRESS_ASYNC(int? userId, string searchTerm, bool? statusId)
        {
            var list = new List<UserAddress>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_UserAddress_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userId", userId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@searchTerm", searchTerm ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@statusId", statusId ?? (object)DBNull.Value));

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

        public async Task<(int code, string message, int? templateId)> NUEVO_USER_ADDRESS_ASYNC(
            int userAddressUserId,
            int userAddressCountryId,
            int userAddressZIPCode,
            string userAddressDescription,
            bool userAddressIsPrincipal,
            int userAddressCreatorId,
            bool userAddressStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_UserAddress_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userAddressUserId", userAddressUserId));
                    cmd.Parameters.Add(new SqlParameter("@userAddressCountryId", userAddressCountryId));
                    cmd.Parameters.Add(new SqlParameter("@userAddressZIPCode", userAddressZIPCode));
                    cmd.Parameters.Add(new SqlParameter("@userAddressDescription", userAddressDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userAddressIsPrincipal", userAddressIsPrincipal));
                    cmd.Parameters.Add(new SqlParameter("@userAddressCreatorId", userAddressCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@userAddressStatusId", userAddressStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Dirección creada exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar la dirección", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_USER_ADDRESS_ASYNC(
            int userAddressId,
            int userAddressCountryId,
            int userAddressZIPCode,
            string userAddressDescription,
            bool userAddressIsPrincipal,
            int userAddressModificatorId,
            bool userAddressStatusId,
            bool forzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_UserAddress_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userAddressId", userAddressId));
                    cmd.Parameters.Add(new SqlParameter("@userAddressCountryId", userAddressCountryId));
                    cmd.Parameters.Add(new SqlParameter("@userAddressZIPCode", userAddressZIPCode));
                    cmd.Parameters.Add(new SqlParameter("@userAddressDescription", userAddressDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userAddressIsPrincipal", userAddressIsPrincipal));
                    cmd.Parameters.Add(new SqlParameter("@userAddressModificatorId", userAddressModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@userAddressStatusId", userAddressStatusId));
                    cmd.Parameters.Add(new SqlParameter("@ForzarRecuperacion", forzarRecuperacion));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Dirección actualizada exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar la dirección", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_USER_ADDRESS_ASYNC(int userAddressId, int userAddressModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_UserAddress_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userAddressId", userAddressId));
                    cmd.Parameters.Add(new SqlParameter("@userAddressModificatorId", userAddressModificatorId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Dirección eliminada exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar la dirección", ex); }
        }

        private UserAddress MapearEntidadDominio(SqlDataReader dr)
        {
            return new UserAddress
            {
                UserAddressId = dr["userAddressId"] as int?,
                UserAddressUserId = dr["userAddressUserId"] as int?,
                UserAddressCountryId = dr["userAddressCountryId"] as int?,
                UserAddressZIPCode = dr["userAddressZIPCode"] as int?,
                UserAddressDescription = dr["userAddressDescription"]?.ToString() ?? string.Empty,
                UserAddressIsPrincipal = dr["userAddressIsPrincipal"] as bool?,
                UserAddressCreatorId = dr["userAddressCreatorId"] as int?,
                UserAddressCreationDate = dr["userAddressCreationDate"] as DateTime?,
                UserAddressModificatorId = dr["userAddressModificatorId"] as int?,
                UserAddressModificationDate = dr["userAddressModificationDate"] as DateTime?,
                UserAddressStatusId = dr["userAddressStatusId"] as bool?
            };
        }
    }
}