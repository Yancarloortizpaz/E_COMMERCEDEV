using Ecom_Domain;
using Ecom_Infraestructure.Database;
using Microsoft.Data.SqlClient;
using modu.application.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ecom_Infraestructure.Repository
{
    public class SubCategory_Repository : ISubCategoryRepository
    {
        private readonly DB_conection _conection;

        public SubCategory_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<SubCategories>> LISTAR_SUBCATEGORY_ASYNC()
        {
            var list = new List<SubCategories>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_SubCategories_List]", con))
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

        public async Task<IEnumerable<SubCategories>> FILTRAR_SUBCATEGORY_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<SubCategories>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_SubCategories_Filter]", con))
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

        public async Task<(int code, string message, int? templateId)> NUEVO_SUBCATEGORY_ASYNC(
            string subCategoryName,
            string subCategoryDescription,
            int subCategoryCreatorId,
            bool subCategoryStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_SubCategories_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@subCategoryName", subCategoryName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@subCategoryDescription", subCategoryDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@subCategoryCreatorId", subCategoryCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@subCategoryStatusId", subCategoryStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Subcategoría creada exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar la subcategoría", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_SUBCATEGORY_ASYNC(
            int subCategoryId,
            string subCategoryName,
            string subCategoryDescription,
            int subCategoryModificatorId,
            bool subCategoryStatusId,
            bool forzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_SubCategories_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@subCategoryId", subCategoryId));
                    cmd.Parameters.Add(new SqlParameter("@subCategoryName", subCategoryName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@subCategoryDescription", subCategoryDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@subCategoryModificatorId", subCategoryModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@subCategoryStatusId", subCategoryStatusId));
                    cmd.Parameters.Add(new SqlParameter("@ForzarRecuperacion", forzarRecuperacion));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Subcategoría actualizada exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar la subcategoría", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_SUBCATEGORY_ASYNC(int subCategoryId, int subCategoryModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_SubCategories_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@subCategoryId", subCategoryId));
                    cmd.Parameters.Add(new SqlParameter("@subCategoryModificatorId", subCategoryModificatorId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Subcategoría eliminada exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar la subcategoría", ex); }
        }

        private SubCategories MapearEntidadDominio(SqlDataReader dr)
        {
            return new SubCategories
            {
                SubCategoryId = dr["subCategoryId"] as int?,
                SubCategoryName = dr["subCategoryName"]?.ToString() ?? string.Empty,
                SubCategoryDescription = dr["subCategoryDescription"]?.ToString() ?? string.Empty,
                SubCategoryCreatorId = dr["subCategoryCreatorId"] as int?,
                SubCategoryCreationDate = dr["subCategoryCreationDate"] as DateTime?,
                SubCategoryModificatorId = dr["subCategoryModificatorId"] as int?,
                SubCategoryModificationDate = dr["subCategoryModificationDate"] as DateTime?,
                SubCategoryStatusId = dr["subCategoryStatusId"] as bool?
            };
        }
    }
}