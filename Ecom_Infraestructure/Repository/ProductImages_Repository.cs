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
    public class ProductImages_Repository : IProductImagesRepository
    {
        private readonly DB_conection _conection;

        public ProductImages_Repository(DB_conection conection)
        {
            _conection = conection;
        }

       
        public async Task<IEnumerable<ProductImages>> LISTAR_PRODUCTIMAGES_ASYNC()
        {
            var list = new List<ProductImages>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();


                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_ProductImages_Filter]", con))
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

        public async Task<IEnumerable<ProductImages>> OBTENER_POR_PRODUCTO_PRODUCTIMAGES_ASYNC(int ProductId)
        {
            var list = new List<ProductImages>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_ProductImages_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@productImageProductId", ProductId));

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

        public async Task<(int code, string message, int? templateId)> NUEVO_PRODUCTIMAGES_ASYNC(
            int productImageProductId,
            string productImageURL,
            string productImageDescription,
            bool productImageIsPrincipal,
            int productImageCreatorId,
            bool productImageStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_ProductImages_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productImageProductId", productImageProductId));
                    cmd.Parameters.Add(new SqlParameter("@productImageURL", productImageURL ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productImageDescription", productImageDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productImageIsPrincipal", productImageIsPrincipal));
                    cmd.Parameters.Add(new SqlParameter("@productImageCreatorId", productImageCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@productImageStatusId", productImageStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Creado con éxito";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar la imagen", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTIMAGES_ASYNC(
            int productImageId,
            int productImageProductId,
            string productImageURL,
            string productImageDescription,
            bool productImageIsPrincipal,
            int productImageModificatorId,
            bool productImageStatusId,
            bool ForzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_ProductImages_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productImageId", productImageId));
                    cmd.Parameters.Add(new SqlParameter("@productImageProductId", productImageProductId));
                    cmd.Parameters.Add(new SqlParameter("@productImageURL", productImageURL ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productImageDescription", productImageDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productImageIsPrincipal", productImageIsPrincipal));
                    cmd.Parameters.Add(new SqlParameter("@productImageCreatorId", productImageModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productImageModificatorId", productImageModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productImageStatusId", productImageStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Actualizado con éxito";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar la imagen", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTIMAGES_ASYNC(int productImageId, int productImageModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                int productId = 0;
                string url = string.Empty;
                string desc = string.Empty;
                bool isPrincipal = false;

                using (SqlCommand cmdFilter = new SqlCommand("[SQM_GENERAL].[sp_ProductImages_Filter]", con))
                {
                    cmdFilter.CommandType = CommandType.StoredProcedure;
                    cmdFilter.Parameters.Add(new SqlParameter("@productImageId", productImageId));
                    using (SqlDataReader dr = await cmdFilter.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            productId = dr["productImageProductId"] as int? ?? 0;
                            url = dr["productImageURL"]?.ToString() ?? string.Empty;
                            desc = dr["productImageDescription"]?.ToString() ?? string.Empty;
                            isPrincipal = dr["productImageIsPrincipal"] as bool? ?? false;
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_ProductImages_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productImageId", productImageId));
                    cmd.Parameters.Add(new SqlParameter("@productImageProductId", productId));
                    cmd.Parameters.Add(new SqlParameter("@productImageURL", url));
                    cmd.Parameters.Add(new SqlParameter("@productImageDescription", desc));
                    cmd.Parameters.Add(new SqlParameter("@productImageIsPrincipal", isPrincipal));
                    cmd.Parameters.Add(new SqlParameter("@productImageCreatorId", productImageModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productImageModificatorId", productImageModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productImageStatusId", false));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Desactivado con éxito";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar (deshabilitar) la imagen", ex); }
        }

        private ProductImages MapearEntidadDominio(SqlDataReader dr)
        {
            return new ProductImages
            {
                productImageId = dr["productImageId"] as int?,
                productImageProductId = dr["productImageProductId"] as int?,
                productImageURL = dr["productImageURL"]?.ToString() ?? string.Empty,
                productImageDescription = dr["productImageDescription"]?.ToString() ?? string.Empty,
                productImageIsPrincipal = dr["productImageIsPrincipal"] as bool?,
                productImageCreatorId = dr["productImageCreatorId"] as int?,
                productImageCreationDate = dr["productImageCreationDate"] as DateTime?,
                productImageModificatorId = dr["productImageModificatorId"] as int?,
                productImageModificationDate = dr["productImageModificationDate"] as DateTime?,
                productImageStatusId = dr["productImageStatusId"] as bool?
            };
        }
    }
}