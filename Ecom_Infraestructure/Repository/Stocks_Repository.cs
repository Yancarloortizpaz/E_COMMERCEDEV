using Ecom_Aplication.Interfaces;
using Ecom_Domain;
using Ecom_Infraestructure.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ecom_Infraestructure.Repository
{
    public class Stocks_Repository : IStocksRepository
    {
        private readonly DB_conection _conection;

        public Stocks_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<Stocks>> LISTAR_STOCKS_ASYNC()
        {
            var list = new List<Stocks>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Stocks_List]", con))
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

        public async Task<IEnumerable<Stocks>> FILTRAR_STOCKS_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<Stocks>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Stocks_Filter]", con))
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

        public async Task<IEnumerable<Stocks>> OBTENER_POR_PRODUCTVARIABLE_STOCKS_ASYNC(int ProductVariableId)
        {
            var list = new List<Stocks>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Stocks_GetByProductVariable]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@productVariableId", ProductVariableId));

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

        public async Task<(int code, string message, bool? templateId)> NUEVO_STOCKS_ASYNC(int stockProductVariableId,int stockQuantity,DateTime stockFactoryDate, DateTime stockExpirationDate,int stockCreatorId, bool stockStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Stocks_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@stockProductVariableId", stockProductVariableId));
                    cmd.Parameters.Add(new SqlParameter("@stockQuantity", stockQuantity));
                    cmd.Parameters.Add(new SqlParameter("@stockCreatorId", stockCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@stockStatusId", stockStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Stock registrado exitosamente.";

                    bool? templateId = null;
                    if (oTemplate.Value != DBNull.Value)
                    {
                        templateId = Convert.ToInt32(oTemplate.Value) > 0;
                    }

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
        }

        public async Task<(int code, string message, bool? templateId)> ACTUALIZAR_STOCKS_ASYNC(
            int stockId,
            int stockQuantityAdjustment,
            int stockModificatorId
        )
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Stocks_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@stockId", stockId));
                    cmd.Parameters.Add(new SqlParameter("@stockQuantityAdjustment", stockQuantityAdjustment));
                   
                    cmd.Parameters.Add(new SqlParameter("@stockModificatorId", stockModificatorId));
                

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Stock actualizado exitosamente.";

                    bool? templateId = null;
                    if (oTemplate.Value != DBNull.Value)
                    {
                        templateId = Convert.ToInt32(oTemplate.Value) > 0;
                    }

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
        }

        public async Task<(int code, string message, bool? templateId)> ELIMINAR_STOCKS_ASYNC(
            int stockId,
            int stockModificatorId
        )
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Stocks_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@stockId", stockId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Stock eliminado exitosamente.";

                    bool? templateId = null;
                    if (oTemplate.Value != DBNull.Value)
                    {
                        templateId = Convert.ToInt32(oTemplate.Value) > 0;
                    }

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
        }

        private Stocks MapearEntidadDominio(SqlDataReader dr)
        {
            return new Stocks
            {
                stockId = dr["stockId"] as int?,
                stockProductVariableId = dr["stockProductVariableId"] as int?,
                stockQuantity = dr["stockQuantity"] as int? ?? 0,
                stockCreatorId = dr["stockCreatorId"] as int?,
                stockCreationDate = dr["stockCreationDate"] as DateTime?,
                stockModificatorId = dr["stockModificatorId"] as int?,
                stockModificationDate = dr["stockModificationDate"] as DateTime?,
                stockStatusId = dr["stockStatusId"] as bool?
            };
        }
    }
}