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
    public class StockMovementDetails_Repository : IStockMovementDetails
    {
        private readonly DB_conection _conection;

        public StockMovementDetails_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<StockMovementDetails>> LISTAR_STOCKMOVEMENTDETAILS_ASYNC()
        {
            var list = new List<StockMovementDetails>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_StockMovementDetails_List]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(MapearEntidadDominio(dr, includeModifier: false));
                        }
                    }
                }
                return list;
            }
            catch (SqlException) { throw; }
        }

        public async Task<IEnumerable<StockMovementDetails>> FILTRAR_STOCKMOVEMENTDETAILS_ASYNC(int? movementId, string searchTerm)
        {
            var list = new List<StockMovementDetails>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_StockMovementDetails_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MovementId", movementId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@SearchTerm", searchTerm ?? (object)DBNull.Value));

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(MapearEntidadDominio(dr, includeModifier: false));
                        }
                    }
                }
                return list;
            }
            catch (SqlException) { throw; }
        }

        public async Task<(int code, string message, int? templateId)> NUEVO_STOCKMOVEMENTDETAILS_ASYNC(
            int stockMovementDetailMovementId,
            int? stockMovementDetailOrderDetailId,
            int? stockMovementDetailStockId,
            int stockMovementDetailQuantity,
            DateTime? stockMovementDetailFactoryDate,
            DateTime? stockMovementDetailExpirationDate,
            int stockMovementDetailCreatorId,
            bool stockMovementDetailStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_StockMovementDetails_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@stockMovementDetailMovementId", stockMovementDetailMovementId));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementDetailOrderDetailId", stockMovementDetailOrderDetailId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementDetailStockId", stockMovementDetailStockId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementDetailQuantity", stockMovementDetailQuantity));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementDetailFactoryDate", stockMovementDetailFactoryDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementDetailExpirationDate", stockMovementDetailExpirationDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementDetailCreatorId", stockMovementDetailCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementDetailStatusId", stockMovementDetailStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Detalle de movimiento de inventario registrado y stock actualizado automáticamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el detalle de movimiento de inventario", ex); }
        }

        private StockMovementDetails MapearEntidadDominio(SqlDataReader dr, bool includeModifier)
        {
            return new StockMovementDetails
            {
                stockMovementDetailId = dr["stockMovementDetailId"] as int?,
                stockMovementDetailMovementId = dr["stockMovementDetailMovementId"] as int?,
                stockMovementDetailOrderDetailId = dr["stockMovementDetailOrderDetailId"] as int?,
                stockMovementDetailStockId = dr["stockMovementDetailStockId"] as int?,
                stockMovementDetailQuantity = dr["stockMovementDetailQuantity"] as int?,
                stockMovementDetailFactoryDate = dr["stockMovementDetailFactoryDate"] as DateTime?,
                stockMovementDetailExpirationDate = dr["stockMovementDetailExpirationDate"] as DateTime?,
                stockMovementDetailCreatorId = HasColumn(dr, "stockMovementDetailCreatorId") ? dr["stockMovementDetailCreatorId"] as int? : null,
                stockMovementDetailCreationDate = dr["stockMovementDetailCreationDate"] as DateTime?,
                stockMovementDetailModifierId = null, 
                stockMovementDetailModificationDate = null, 
                stockMovementDetailStatusId = dr["stockMovementDetailStatusId"] as bool?
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