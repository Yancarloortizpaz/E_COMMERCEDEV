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
    public class StockMovements_Repository : IStockMovements
    {
        private readonly DB_conection _conection;

        public StockMovements_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<StockMovements>> LISTAR_STOCKMOVEMENTS_ASYNC()
        {
            var list = new List<StockMovements>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_StockMovements_List]", con))
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

        public async Task<IEnumerable<StockMovements>> FILTRAR_STOCKMOVEMENTS_ASYNC(string searchTerm)
        {
            var list = new List<StockMovements>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_StockMovements_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@SearchTerm", searchTerm ?? (object)DBNull.Value));

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

        public async Task<(int code, string message, int? templateId)> NUEVO_STOCKMOVEMENTS_ASYNC(
            int stockMovementType,
            int? stockMovementOrderId,
            string stockMovementReference,
            DateTime stockMovementDate,
            int stockMovementCreatorId,
            int stockMovementStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_StockMovements_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@stockMovementType", stockMovementType));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementOrderId", stockMovementOrderId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementReference", stockMovementReference ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementDate", stockMovementDate));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementCreatorId", stockMovementCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementStatusId", stockMovementStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Movimiento de inventario creado correctamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el movimiento de inventario", ex); }
        }

        private StockMovements MapearEntidadDominio(SqlDataReader dr)
        {
            return new StockMovements
            {
                stockMovementId = dr["stockMovementId"] as int?,
                stockMovementType = dr["stockMovementType"] as int?,
                stockMovementOrderId = dr["stockMovementOrderId"] as int?,
                stockMovementReference = dr["stockMovementReference"]?.ToString() ?? string.Empty,
                stockMovementDate = dr["stockMovementDate"] as DateTime?,
                stockMovementCreatorId = HasColumn(dr, "stockMovementCreatorId") ? dr["stockMovementCreatorId"] as int? : null,
                stockMovementCreationDate = dr["stockMovementCreationDate"] as DateTime?,
                stockMovementModifierId = null, 
                stockMovementModificationDate = null, 
                stockMovementStatusId = dr["stockMovementStatusId"] as int?
            };
        }

        private bool HasColumn(SqlDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
