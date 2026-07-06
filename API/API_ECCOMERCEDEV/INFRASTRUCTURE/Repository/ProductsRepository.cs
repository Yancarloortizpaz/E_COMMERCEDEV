using APLICATION.Interfaces;
using DOMAIN.Products;
using DOMAIN.VariablesSalida;
using INFRASTRUCTURE.DB;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly DBconexionfactory _connection;

        public ProductsRepository(DBconexionfactory connection)
        {
            _connection = connection;
        }

        #region escritura_products

        public async Task<OUTPUT> Insertar_ProductsAsync(DM_Products_insertar modelo)
        {
            var result = new OUTPUT();
            try
            {
                using var con = _connection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Products_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productName", modelo.productName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productDescription", modelo.productDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productProductIdentificatorId", modelo.productProductIdentificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productMarkByProviderId", modelo.productMarkByProviderId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productCreatorId", modelo.productCreatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productStatusId", modelo.productStatusId ?? (object)DBNull.Value));

                    SqlParameter pCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter pMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter pTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pCode);
                    cmd.Parameters.Add(pMessage);
                    cmd.Parameters.Add(pTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    result.Code = pCode.Value != DBNull.Value ? (int?)pCode.Value : null;
                    result.Message = pMessage.Value != DBNull.Value ? pMessage.Value.ToString() : null;
                    result.TemplateId = pTemplate.Value != DBNull.Value ? (int?)pTemplate.Value : null;
                }
                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en el motor SQL al registrar el producto.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico de infraestructura al registrar el producto.", ex);
            }
        }

        public async Task<OUTPUT> Editar_ProductsAsync(DM_Products_actualizar modelo)
        {
            var result = new OUTPUT();
            try
            {
                using var con = _connection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Products_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productId", modelo.productId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productName", modelo.productName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productDescription", modelo.productDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productProductIdentificatorId", modelo.productProductIdentificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productMarkByProviderId", modelo.productMarkByProviderId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productModificatorId", modelo.productModificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productStatusId", modelo.productStatusId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@ForzarRecuperacion", modelo.ForzarRecuperacion ?? (object)DBNull.Value));

                    SqlParameter pCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter pMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter pTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pCode);
                    cmd.Parameters.Add(pMessage);
                    cmd.Parameters.Add(pTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    result.Code = pCode.Value != DBNull.Value ? (int?)pCode.Value : null;
                    result.Message = pMessage.Value != DBNull.Value ? pMessage.Value.ToString() : null;
                    result.TemplateId = pTemplate.Value != DBNull.Value ? (int?)pTemplate.Value : null;
                }
                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en el motor SQL al actualizar el producto.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico de infraestructura al actualizar el producto.", ex);
            }
        }

        public async Task<OUTPUT> Eliminar_ProductsAsync(int? productId, int? productModificatorId)
        {
            var result = new OUTPUT();
            try
            {
                using var con = _connection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Products_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productId", productId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productModificatorId", productModificatorId ?? (object)DBNull.Value));

                    SqlParameter pCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter pMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter pTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pCode);
                    cmd.Parameters.Add(pMessage);
                    cmd.Parameters.Add(pTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    result.Code = pCode.Value != DBNull.Value ? (int?)pCode.Value : null;
                    result.Message = pMessage.Value != DBNull.Value ? pMessage.Value.ToString() : null;
                    result.TemplateId = pTemplate.Value != DBNull.Value ? (int?)pTemplate.Value : null;
                }
                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en el motor SQL al eliminar de forma lógica el producto.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico de infraestructura al eliminar el producto.", ex);
            }
        }

        #endregion

        #region lectura_products

        public async Task<IEnumerable<DM_Products_listar>> Listar_ProductsAsync()
        {
            var list = new List<DM_Products_listar>();
            try
            {
                using var con = _connection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Products_List]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(MapearDataReaderAListar(dr));
                        }
                    }
                }
                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al consultar el listado completo de productos en la base de datos.", ex);
            }
        }

        public async Task<IEnumerable<DM_Products_filtrar>> Filtrar_ProductsAsync(string? searchTerm)
        {
            var list = new List<DM_Products_filtrar>();
            try
            {
                using var con = _connection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_Products_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@SearchTerm", searchTerm ?? (object)DBNull.Value));

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(MapearDataReaderAFiltrar(dr));
                        }
                    }
                }
                return list;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al filtrar productos en la base de datos.", ex);
            }
        }

        #endregion

        #region mapeadores

        private DM_Products_listar MapearDataReaderAListar(SqlDataReader dr)
        {
            return new DM_Products_listar
            {
                productId = dr["productId"] != DBNull.Value ? (int?)dr["productId"] : null,
                productName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
                productDescription = dr["productDescription"] != DBNull.Value ? dr["productDescription"].ToString() : null,
                productIdentificatorId = dr["productIdentificatorId"] != DBNull.Value ? (int?)dr["productIdentificatorId"] : null,
                categoryId = dr["categoryId"] != DBNull.Value ? (int?)dr["categoryId"] : null,
                categoryName = dr["categoryName"] != DBNull.Value ? dr["categoryName"].ToString() : null,
                subCategoryId = dr["subCategoryId"] != DBNull.Value ? (int?)dr["subCategoryId"] : null,
                subCategoryName = dr["subCategoryName"] != DBNull.Value ? dr["subCategoryName"].ToString() : null,
                segmentId = dr["segmentId"] != DBNull.Value ? (int?)dr["segmentId"] : null,
                segmentName = dr["segmentName"] != DBNull.Value ? dr["segmentName"].ToString() : null,
                markByProviderId = dr["markByProviderId"] != DBNull.Value ? (int?)dr["markByProviderId"] : null,
                markId = dr["markId"] != DBNull.Value ? (int?)dr["markId"] : null,
                markName = dr["markName"] != DBNull.Value ? dr["markName"].ToString() : null,
                providerId = dr["providerId"] != DBNull.Value ? (int?)dr["providerId"] : null,
                providerName = dr["providerName"] != DBNull.Value ? dr["providerName"].ToString() : null,
                statusId = dr["statusId"] != DBNull.Value ? (bool?)dr["statusId"] : null
            };
        }

        private DM_Products_filtrar MapearDataReaderAFiltrar(SqlDataReader dr)
        {
            return new DM_Products_filtrar
            {
                productId = dr["productId"] != DBNull.Value ? (int?)dr["productId"] : null,
                productName = dr["productName"] != DBNull.Value ? dr["productName"].ToString() : null,
                productDescription = dr["productDescription"] != DBNull.Value ? dr["productDescription"].ToString() : null,
                productIdentificatorId = dr["productIdentificatorId"] != DBNull.Value ? (int?)dr["productIdentificatorId"] : null,
                categoryId = dr["categoryId"] != DBNull.Value ? (int?)dr["categoryId"] : null,
                categoryName = dr["categoryName"] != DBNull.Value ? dr["categoryName"].ToString() : null,
                subCategoryId = dr["subCategoryId"] != DBNull.Value ? (int?)dr["subCategoryId"] : null,
                subCategoryName = dr["subCategoryName"] != DBNull.Value ? dr["subCategoryName"].ToString() : null,
                segmentId = dr["segmentId"] != DBNull.Value ? (int?)dr["segmentId"] : null,
                segmentName = dr["segmentName"] != DBNull.Value ? dr["segmentName"].ToString() : null,
                markByProviderId = dr["markByProviderId"] != DBNull.Value ? (int?)dr["markByProviderId"] : null,
                markId = dr["markId"] != DBNull.Value ? (int?)dr["markId"] : null,
                markName = dr["markName"] != DBNull.Value ? dr["markName"].ToString() : null,
                providerId = dr["providerId"] != DBNull.Value ? (int?)dr["providerId"] : null,
                providerName = dr["providerName"] != DBNull.Value ? dr["providerName"].ToString() : null,
                statusId = dr["statusId"] != DBNull.Value ? (bool?)dr["statusId"] : null
            };
        }

        #endregion
    }
}
