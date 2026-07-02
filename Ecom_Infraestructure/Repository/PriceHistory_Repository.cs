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
    public class PriceHistory_Repository : IPriceHistory
    {
        private readonly DB_conection _conection;

        public PriceHistory_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<PriceHistory_DTOS>> LISTAR_PRICEHISTORY_ASYNC()
        {
            var list = new List<PriceHistory_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_PriceHistory_List]", con))
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

        public async Task<IEnumerable<PriceHistory_DTOS>> FILTRAR_PRICEHISTORY_ASYNC(
            int? priceHistoryId,
            int? priceHistoryProductVariableId,
            decimal? priceHistoryOldPrice,
            decimal? priceHistoryNewPrice,
            DateTime? priceHistoryChangeDate,
            int? priceHistoryModifierId)
        {
            var list = new List<PriceHistory_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_PriceHistory_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@priceHistoryId", priceHistoryId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryProductVariableId", priceHistoryProductVariableId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryOldPrice", priceHistoryOldPrice ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryNewPrice", priceHistoryNewPrice ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryChangeDate", priceHistoryChangeDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryModifierId", priceHistoryModifierId ?? (object)DBNull.Value));

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

        public async Task<(int code, string message, int? templateId)> NUEVO_PRICEHISTORY_ASYNC(
            int priceHistoryProductVariableId,
            decimal priceHistoryOldPrice,
            decimal priceHistoryNewPrice,
            int? priceHistoryModifierId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_PriceHistory_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@priceHistoryProductVariableId", priceHistoryProductVariableId));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryOldPrice", priceHistoryOldPrice));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryNewPrice", priceHistoryNewPrice));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryModifierId", priceHistoryModifierId ?? (object)DBNull.Value));

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
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el historial de precio", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRICEHISTORY_ASYNC(
            int priceHistoryId,
            int priceHistoryProductVariableId,
            decimal priceHistoryOldPrice,
            decimal priceHistoryNewPrice,
            int? priceHistoryModifierId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_PriceHistory_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@priceHistoryId", priceHistoryId));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryProductVariableId", priceHistoryProductVariableId));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryOldPrice", priceHistoryOldPrice));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryNewPrice", priceHistoryNewPrice));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryModifierId", priceHistoryModifierId ?? (object)DBNull.Value));

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
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el historial de precio", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_PRICEHISTORY_ASYNC(
            int priceHistoryId,
            int priceHistoryProductVariableId,
            decimal priceHistoryOldPrice,
            decimal priceHistoryNewPrice,
            int? priceHistoryModifierId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_PriceHistory_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@priceHistoryId", priceHistoryId));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryProductVariableId", priceHistoryProductVariableId));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryOldPrice", priceHistoryOldPrice));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryNewPrice", priceHistoryNewPrice));
                    cmd.Parameters.Add(new SqlParameter("@priceHistoryModifierId", priceHistoryModifierId ?? (object)DBNull.Value));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Campos del registro actualizados en borrado lógico.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el historial de precio", ex); }
        }

        private PriceHistory_DTOS MapearDTO(SqlDataReader dr)
        {
            return new PriceHistory_DTOS
            {
                priceHistoryId = dr["priceHistoryId"] as int?,
                priceHistoryProductVariableId = dr["priceHistoryProductVariableId"] as int?,
                priceHistoryOldPrice = dr["priceHistoryOldPrice"] as decimal?,
                priceHistoryNewPrice = dr["priceHistoryNewPrice"] as decimal?,
                priceHistoryChangeDate = dr["priceHistoryChangeDate"] as DateTime?,
                priceHistoryModifierId = dr["priceHistoryModifierId"] as int?
            };
        }
    }
}