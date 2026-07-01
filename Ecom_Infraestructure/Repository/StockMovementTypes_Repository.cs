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
    public class StockMovementTypes_Repository : IStockMovementTypes
    {
        private readonly DB_conection _conection;

        public StockMovementTypes_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<StockMovementTypes>> FILTRAR_STOCKMOVEMENTTYPES_ASYNC(
            int? stockMovementTypeId,
            string stockMovementTypeName,
            string stockMovementTypeDescription,
            int? stockMovementTypeCreatorId,
            DateTime? stockMovementTypeCreationDate,
            int? stockMovementTypeModificatorId,
            DateTime? stockMovementTypeModificationDate,
            bool? stockMovementTypeStatusId)
        {
            var list = new List<StockMovementTypes>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_StockMovementTypes_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@stockMovementTypeId", stockMovementTypeId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementTypeName", stockMovementTypeName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementTypeDescription", stockMovementTypeDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementTypeCreatorId", stockMovementTypeCreatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementTypeCreationDate", stockMovementTypeCreationDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementTypeModificatorId", stockMovementTypeModificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementTypeModificationDate", stockMovementTypeModificationDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@stockMovementTypeStatusId", stockMovementTypeStatusId ?? (object)DBNull.Value));

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

        private StockMovementTypes MapearEntidadDominio(SqlDataReader dr)
        {
            return new StockMovementTypes
            {
                stockMovementTypeId = dr["stockMovementTypeId"] as int?,
                stockMovementTypeName = dr["stockMovementTypeName"]?.ToString() ?? string.Empty,
                stockMovementTypeDescription = dr["stockMovementTypeDescription"]?.ToString() ?? string.Empty,
                stockMovementTypeCreatorId = dr["stockMovementTypeCreatorId"] as int?,
                stockMovementTypeCreationDate = dr["stockMovementTypeCreationDate"] as DateTime?,
                stockMovementTypeModificatorId = dr["stockMovementTypeModificatorId"] as int?,
                stockMovementTypeModificationDate = dr["stockMovementTypeModificationDate"] as DateTime?,
                stockMovementTypeStatusId = dr["stockMovementTypeStatusId"] as bool?
            };
        }
    }
}