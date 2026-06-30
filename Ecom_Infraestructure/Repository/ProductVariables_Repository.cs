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
    public class ProductVariables_Repository : IProductVariablesRepository
    {
        private readonly DB_conection _conection;

        public ProductVariables_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<ProductVariables>> LISTAR_PRODUCTVARIABLES_ASYNC()
        {
            var list = new List<ProductVariables>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                
                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_ProductVariables_Filter]", con))
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

        public async Task<IEnumerable<ProductVariables>> FILTRAR_PRODUCTVARIABLES_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<ProductVariables>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_ProductVariables_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Ajustamos los parámetros mapeando al filtro correspondiente de tu SP
                    cmd.Parameters.Add(new SqlParameter("@searchTerm", searchTerm ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productVariableStatusId", statusId ?? (object)DBNull.Value));

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

        public async Task<(int code, string message, int? templateId)> NUEVO_PRODUCTVARIABLES_ASYNC(
            int productVariableProductId,
            string productVariableValue,
            decimal productVariablePrice,
            int productVariableCurrencyId,
            int productVariableCreatorId,
            bool productVariableStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_ProductVariables_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productVariableProductId", productVariableProductId));
                    cmd.Parameters.Add(new SqlParameter("@productVariableValue", productVariableValue ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productVariablePrice", productVariablePrice));
                    cmd.Parameters.Add(new SqlParameter("@productVariableCurrencyId", productVariableCurrencyId));
                    cmd.Parameters.Add(new SqlParameter("@productVariableCreatorId", productVariableCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@productVariableStatusId", productVariableStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Variante creada exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar la variante de producto", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTVARIABLES_ASYNC(
            int productVariableId,
            int productVariableProductId,
            string productVariableValue,
            decimal productVariablePrice,
            int productVariableCurrencyId,
            int productVariableModificatorId,
            bool productVariableStatusId,
            bool ForzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_ProductVariables_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productVariableId", productVariableId));
                    cmd.Parameters.Add(new SqlParameter("@productVariableProductId", productVariableProductId));
                    cmd.Parameters.Add(new SqlParameter("@productVariableValue", productVariableValue ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productVariablePrice", productVariablePrice));
                    cmd.Parameters.Add(new SqlParameter("@productVariableCurrencyId", productVariableCurrencyId));
                    cmd.Parameters.Add(new SqlParameter("@productVariableModificatorId", productVariableModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productVariableStatusId", productVariableStatusId));
                    cmd.Parameters.Add(new SqlParameter("@ForzarRecuperacion", ForzarRecuperacion));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Variante actualizada exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar la variante de producto", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTVARIABLES_ASYNC(int productVariableId, int productVariableModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_ProductVariables_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@productVariableId", productVariableId));
                    cmd.Parameters.Add(new SqlParameter("@productVariableModificatorId", productVariableModificatorId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Variante eliminada exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar la variante de producto", ex); }
        }

        private ProductVariables MapearEntidadDominio(SqlDataReader dr)
        {
            return new ProductVariables
            {
                productVariableId = dr["productVariableId"] as int?,
                productVariableProductId = dr["productVariableProductId"] as int?,
                productVariableValue = dr["productVariableValue"]?.ToString() ?? string.Empty,
                productVariablePrice = dr["productVariablePrice"] as decimal? ?? 0xc,
                productVariableCurrencyId = dr["productVariableCurrencyId"] as int?,
                productVariableCreatorId = dr["productVariableCreatorId"] as int?,
                productVariableCreationDate = dr["productVariableCreationDate"] as DateTime?,
                productVariableModificatorId = dr["productVariableModificatorId"] as int?,
                productVariableModificationDate = dr["productVariableModificationDate"] as DateTime?,
                productVariableStatusId = dr["productVariableStatusId"] as bool?
            };
        }
    }
}