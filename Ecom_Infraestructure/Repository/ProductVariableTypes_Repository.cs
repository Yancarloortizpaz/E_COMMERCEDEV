using Ecom_Aplication.Interfaces;
using Ecom_Domain; // Asegura que aquí esté tu clase de dominio ProductVariableTypes
using Ecom_Infraestructure.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ecom_Infraestructure.Repository
{
    public class ProductVariableTypes_Repository : IProductVariableTypesRepository
    {
        private readonly DB_conection _conection;

        public ProductVariableTypes_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<ProductVariableTypes>> LISTAR_PRODUCTVARIABLETYPES_ASYNC()
        {
            var list = new List<ProductVariableTypes>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_ProductVariableTypes_List]", con))
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

        public async Task<IEnumerable<ProductVariableTypes>> FILTRAR_PRODUCTVARIABLETYPES_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<ProductVariableTypes>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_ProductVariableTypes_Filter]", con))
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

        public async Task<(int code, string message, int? templateId)> NUEVO_PRODUCTVARIABLETYPES_ASYNC(
            string productVariableTypeName,
            string productVariableTypeDescription,
            int productVariableTypeCreatorId,
            bool productVariableTypeStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_ProductVariableTypes_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productVariableTypeName", productVariableTypeName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productVariableTypeDescription", productVariableTypeDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productVariableTypeCreatorId", productVariableTypeCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@productVariableTypeStatusId", productVariableTypeStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Tipo de variable creado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el tipo de variable", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PRODUCTVARIABLETYPES_ASYNC(
            int productVariableTypeId,
            string productVariableTypeName,
            string productVariableTypeDescription,
            int productVariableTypeModificatorId,
            bool productVariableTypeStatusId,
            bool ForzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_ProductVariableTypes_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@productVariableTypeId", productVariableTypeId));
                    cmd.Parameters.Add(new SqlParameter("@productVariableTypeName", productVariableTypeName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productVariableTypeDescription", productVariableTypeDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@productVariableTypeModificatorId", productVariableTypeModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@productVariableTypeStatusId", productVariableTypeStatusId));
                    cmd.Parameters.Add(new SqlParameter("@ForzarRecuperacion", ForzarRecuperacion));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Tipo de variable actualizado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el tipo de variable", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_PRODUCTVARIABLETYPES_ASYNC(int productVariableTypeId, int productVariableTypeModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_ProductVariableTypes_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@productVariableTypeId", productVariableTypeId));
                    cmd.Parameters.Add(new SqlParameter("@productVariableTypeModificatorId", productVariableTypeModificatorId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Tipo de variable eliminado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el tipo de variable", ex); }
        }

        private ProductVariableTypes MapearEntidadDominio(SqlDataReader dr)
        {
            return new ProductVariableTypes
            {
                productVariableTypeId = dr["productVariableTypeId"] as int?,
                productVariableTypeName = dr["productVariableTypeName"]?.ToString() ?? string.Empty,
                productVariableTypeDescription = dr["productVariableTypeDescription"]?.ToString() ?? string.Empty,
                productVariableTypeCreatorId = dr["productVariableTypeCreatorId"] as int?,
                productVariableTypeCreationDate = dr["productVariableTypeCreationDate"] as DateTime?,
                productVariableTypeModificatorId = dr["productVariableTypeModificatorId"] as int?,
                productVariableTypeModificationDate = dr["productVariableTypeModificationDate"] as DateTime?,
                productVariableTypeStatusId = dr["productVariableTypeStatusId"] as bool?
            };
        }
    }
}