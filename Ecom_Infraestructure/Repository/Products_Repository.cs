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
    public class Products_Repository : IProductsRepository
    {
        private readonly DB_conection _conection;

        public Products_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<Products>> LISTAR_PRODUCTS_ASYNC()
        {
            var list = new List<Products>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Products_List]", con))
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

        public async Task<IEnumerable<Products>> FILTRAR_PRODUCTS_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<Products>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Products_Filter]", con))
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

        public async Task<(int code, string message, int? templateId)> NUEVO_PRODUCTS_ASYNC(
            string productName,
            string productDescription,
            int productProductIdentificatorId,
            int productMarkByProviderId,
            int productCreatorId,
            bool productStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Products_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productName", productName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productDescription", productDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productProductIdentificatorId", productProductIdentificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productMarkByProviderId", productMarkByProviderId));
                    cmd.Parameters.Add(new SqlParameter("@productCreatorId", productCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@productStatusId", productStatusId));

                    // Parámetros OUTPUT estándar del SP
                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Producto creado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el producto", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTS_ASYNC(
            int productId,
            string productName,
            string productDescription,
            int productProductIdentificatorId,
            int productMarkByProviderId,
            int productModificatorId,
            bool productStatusId,
            bool ForzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Products_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productId", productId));
                    cmd.Parameters.Add(new SqlParameter("@productName", productName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productDescription", productDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productProductIdentificatorId", productProductIdentificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productMarkByProviderId", productMarkByProviderId));
                    cmd.Parameters.Add(new SqlParameter("@productModificatorId", productModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productStatusId", productStatusId));
                    cmd.Parameters.Add(new SqlParameter("@ForzarRecuperacion", ForzarRecuperacion));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Producto actualizado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el producto", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTS_ASYNC(int productId, int productModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Products_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@productId", productId));
                    cmd.Parameters.Add(new SqlParameter("@productModificatorId", productModificatorId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Producto eliminado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el producto", ex); }
        }

        private Products MapearEntidadDominio(SqlDataReader dr)
        {
            return new Products
            {
                productId = dr["productId"] as int?,
                productName = dr["productName"]?.ToString() ?? string.Empty,
                productDescription = dr["productDescription"]?.ToString() ?? string.Empty,
                productProductIdentificatorId = dr["productProductIdentificatorId"] as int?,
                productMarkByProviderId = dr["productMarkByProviderId"] as int?,
                productCreatorId = dr["productCreatorId"] as int?,
                productCreationDate = dr["productCreationDate"] as DateTime?,
                productModificatorId = dr["productModificatorId"] as int?,
                productModificationDate = dr["productModificationDate"] as DateTime?,
                productStatusId = dr["productStatusId"] as bool?
            };
        }
    }
}
