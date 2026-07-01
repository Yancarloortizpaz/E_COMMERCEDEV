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
    public class ProductIdentificators_Repository : IProductIdentificators
    {
        private readonly DB_conection _conection;

        public ProductIdentificators_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<ProductIdentificators_DTOS>> LISTAR_PRODUCTIDENTIFICATORS_ASYNC()
        {
            var list = new List<ProductIdentificators_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_ProductIdentificators_List]", con))
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

        public async Task<IEnumerable<ProductIdentificators_DTOS>> FILTRAR_PRODUCTIDENTIFICATORS_ASYNC(
            int? productIdentificatorId,
            int? productIdentificatorCategoryId,
            int? productIdentificatorSubCategoryId,
            int? productIdentificatorSegmentId,
            int? productIdentificatorCreatorId,
            DateTime? productIdentificatorCreationDate,
            int? productIdentificatorModificatorId,
            DateTime? productIdentificatorModificationDate,
            bool? productIdentificatorStatusId)
        {
            var list = new List<ProductIdentificators_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_ProductIdentificators_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorId", productIdentificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorCategoryId", productIdentificatorCategoryId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorSubCategoryId", productIdentificatorSubCategoryId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorSegmentId", productIdentificatorSegmentId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorCreatorId", productIdentificatorCreatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorCreationDate", productIdentificatorCreationDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorModificatorId", productIdentificatorModificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorModificationDate", productIdentificatorModificationDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorStatusId", productIdentificatorStatusId ?? (object)DBNull.Value));

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

        public async Task<(int code, string message, int? templateId)> NUEVO_PRODUCTIDENTIFICATORS_ASYNC(
            int productIdentificatorCategoryId,
            int productIdentificatorSubCategoryId,
            int productIdentificatorSegmentId,
            int productIdentificatorCreatorId,
            int? productIdentificatorModificatorId,
            bool productIdentificatorStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_ProductIdentificators_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorCategoryId", productIdentificatorCategoryId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorSubCategoryId", productIdentificatorSubCategoryId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorSegmentId", productIdentificatorSegmentId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorCreatorId", productIdentificatorCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorModificatorId", productIdentificatorModificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorStatusId", productIdentificatorStatusId));

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
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el identificador de producto", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTIDENTIFICATORS_ASYNC(
            int productIdentificatorId,
            int productIdentificatorCategoryId,
            int productIdentificatorSubCategoryId,
            int productIdentificatorSegmentId,
            int productIdentificatorCreatorId,
            int productIdentificatorModificatorId,
            bool productIdentificatorStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_ProductIdentificators_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorId", productIdentificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorCategoryId", productIdentificatorCategoryId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorSubCategoryId", productIdentificatorSubCategoryId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorSegmentId", productIdentificatorSegmentId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorCreatorId", productIdentificatorCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorModificatorId", productIdentificatorModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorStatusId", productIdentificatorStatusId));

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
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el identificador de producto", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTIDENTIFICATORS_ASYNC(
            int productIdentificatorId,
            int productIdentificatorCategoryId,
            int productIdentificatorSubCategoryId,
            int productIdentificatorSegmentId,
            int productIdentificatorCreatorId,
            int productIdentificatorModificatorId,
            bool productIdentificatorStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_ProductIdentificators_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorId", productIdentificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorCategoryId", productIdentificatorCategoryId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorSubCategoryId", productIdentificatorSubCategoryId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorSegmentId", productIdentificatorSegmentId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorCreatorId", productIdentificatorCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorModificatorId", productIdentificatorModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productIdentificatorStatusId", productIdentificatorStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Registro actualizado/deshabilitado.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el identificador de producto", ex); }
        }

        private ProductIdentificators_DTOS MapearDTO(SqlDataReader dr)
        {
            return new ProductIdentificators_DTOS
            {
                ProductIdentificatorId = dr["productIdentificatorId"] as int?,
                ProductIdentificatorCategoryId = dr["productIdentificatorCategoryId"] as int?,
                ProductIdentificatorSubCategoryId = dr["productIdentificatorSubCategoryId"] as int?,
                ProductIdentificatorSegmentId = dr["productIdentificatorSegmentId"] as int?,
                ProductIdentificatorCreatorId = dr["productIdentificatorCreatorId"] as int?,
                ProductIdentificatorCreationDate = dr["productIdentificatorCreationDate"] as DateTime?,
                ProductIdentificatorModificatorId = dr["productIdentificatorModificatorId"] as int?,
                ProductIdentificatorModificationDate = dr["productIdentificatorModificationDate"] as DateTime?,
                ProductIdentificatorStatusId = dr["productIdentificatorStatusId"] as bool?
            };
        }
    }
}