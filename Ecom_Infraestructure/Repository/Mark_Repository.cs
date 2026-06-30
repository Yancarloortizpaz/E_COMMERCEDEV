using Ecom_Aplication.Interfaces; 
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
    public class Mark_Repository : IMarkRepository
    {
        private readonly DB_conection _conection;

        public Mark_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<Marks>> LISTAR_MARK_ASYNC()
        {
            var list = new List<Marks>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Marks_List]", con))
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

        public async Task<IEnumerable<Marks>> FILTRAR_MARK_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<Marks>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Marks_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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

        public async Task<(int code, string message, int? templateId)> NUEVO_MARK_ASYNC(
            string markName,
            string markDescription,
            int markCreatorId,
            bool markStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Marks_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@markName", markName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@markDescription", markDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@markCreatorId", markCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@markStatusId", markStatusId));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Marca creada con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al crear la marca", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_MARK_ASYNC(
            int markId,
            string markName,
            string markDescription,
            int markModificatorId,
            bool markStatusId,
            bool forzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Marks_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@markId", markId));
                    cmd.Parameters.Add(new SqlParameter("@markName", markName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@markDescription", markDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@markModificatorId", markModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@markStatusId", markStatusId));
                    cmd.Parameters.Add(new SqlParameter("@forzarRecuperacion", forzarRecuperacion));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Marca actualizada con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar la marca", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_MARK_ASYNC(int markId, int markModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Marks_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@markId", markId));
                    cmd.Parameters.Add(new SqlParameter("@markModificatorId", markModificatorId));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Marca eliminada con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar la marca", ex); }
        }

        private Marks MapearEntidadDominio(SqlDataReader dr)
        {
            return new Marks
            {
                MarkId = dr["MarkId"] as int?,
                MarkName = dr["MarkName"]?.ToString() ?? string.Empty,
                MarkDescription = dr["MarkDescription"]?.ToString() ?? string.Empty,
                MarkCreatorId = dr["MarkCreatorId"] as int?,
                MarkCreationDate = dr["MarkCreationDate"] as DateTime?,
                MarkModificatorId = dr["MarkModificatorId"] as int?,
                MarkModificationDate = dr["MarkModificationDate"] as DateTime?,
                MarkStatusId = dr["MarkStatusId"] as bool?
            };
        }
    }
}
