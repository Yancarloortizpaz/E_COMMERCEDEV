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
    public class Segment_Repository : ISegmentRepository
    {
        private readonly DB_conection _conection;

        public Segment_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<Segments>> LISTAR_SEGMENT_ASYNC()
        {
            var list = new List<Segments>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Segments_List]", con))
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

        public async Task<IEnumerable<Segments>> FILTRAR_SEGMENT_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<Segments>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Segments_Filter]", con))
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

        public async Task<(int code, string message, int? templateId)> NUEVO_SEGMENT_ASYNC(
            string segmentName,
            string segmentDescription,
            int segmentCreatorId,
            bool segmentStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Segments_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@segmentName", segmentName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@segmentDescription", segmentDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@segmentCreatorId", segmentCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@segmentStatusId", segmentStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Segmento creado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el segmento", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_SEGMENT_ASYNC(
            int segmentId,
            string segmentName,
            string segmentDescription,
            int segmentModificatorId,
            bool segmentStatusId,
            bool forzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Segments_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@segmentId", segmentId));
                    cmd.Parameters.Add(new SqlParameter("@segmentName", segmentName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@segmentDescription", segmentDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@segmentModificatorId", segmentModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@segmentStatusId", segmentStatusId));
                    cmd.Parameters.Add(new SqlParameter("@ForzarRecuperacion", forzarRecuperacion));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Segmento actualizado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el segmento", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_SEGMENT_ASYNC(int segmentId, int segmentModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Segments_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@segmentId", segmentId));
                    cmd.Parameters.Add(new SqlParameter("@segmentModificatorId", segmentModificatorId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Segmento eliminado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el segmento", ex); }
        }

        private Segments MapearEntidadDominio(SqlDataReader dr)
        {
            return new Segments
            {
                SegmentId = dr["segmentId"] as int?,
                SegmentName = dr["segmentName"]?.ToString() ?? string.Empty,
                SegmentDescription = dr["segmentDescription"]?.ToString() ?? string.Empty,
                SegmentCreatorId = dr["segmentCreatorId"] as int?,
                SegmentCreationDate = dr["segmentCreationDate"] as DateTime?,
                SegmentModificatorId = dr["segmentModificatorId"] as int?,
                SegmentModificationDate = dr["segmentModificationDate"] as DateTime?,
                SegmentStatusId = dr["segmentStatusId"] as bool?
            };
        }
    }
}