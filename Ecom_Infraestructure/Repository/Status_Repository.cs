using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using Ecom_Infraestructure.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ecom_Infraestructure.Repository
{
    public class Status_Repository : IStatus
    {
        private readonly DB_conection _conection;

        public Status_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<Status_DTOS>> LISTAR_STATUS_ASYNC()
        {
            var list = new List<Status_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Status_List]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(MapearDTO(dr));
                        }
                    }
                }
                return list;
            }
            catch (SqlException) { throw; }
        }

        public async Task<IEnumerable<Status_DTOS>> FILTRAR_STATUS_ASYNC(
            int? statusId,
            string statusName,
            int? statusCreatorId,
            DateTime? statusCreationDate,
            int? statusStatusId)
        {
            var list = new List<Status_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Status_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@statusId", statusId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@statusName", statusName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@statusCreatorId", statusCreatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@statusCreationDate", statusCreationDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@statusStatusId", statusStatusId ?? (object)DBNull.Value));

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(MapearDTO(dr));
                        }
                    }
                }
                return list;
            }
            catch (SqlException) { throw; }
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_STATUS_ASYNC(
            string statusName,
            int statusCreatorId,
            int statusStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Status_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@statusName", statusName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@statusCreatorId", statusCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@statusStatusId", statusStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Registro creado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el estado", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_STATUS_ASYNC(
            int statusId,
            string statusName,
            int statusCreatorId,
            int statusStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Status_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@statusId", statusId));
                    cmd.Parameters.Add(new SqlParameter("@statusName", statusName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@statusCreatorId", statusCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@statusStatusId", statusStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Registro actualizado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el estado", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_STATUS_ASYNC(
            int statusId,
            int statusStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Status_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@statusId", statusId));
                    cmd.Parameters.Add(new SqlParameter("@statusStatusId", statusStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Registro actualizado/deshabilitado.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el estado", ex); }
        }

        private Status_DTOS MapearDTO(SqlDataReader dr)
        {
            return new Status_DTOS
            {
                StatusId = dr["statusId"] as int?,
                StatusName = dr["statusName"]?.ToString() ?? string.Empty,
                StatusCreatorId = dr["statusCreatorId"] as int?,
                StatusCreationDate = dr["statusCreationDate"] as DateTime?,
                StatusStatusId = dr["statusStatusId"] as int?
            };
        }
    }
}