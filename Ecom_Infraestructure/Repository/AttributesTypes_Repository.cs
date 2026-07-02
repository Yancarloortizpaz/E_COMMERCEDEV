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
    public class AttributesType_Repository : IAttributesType
    {
        private readonly DB_conection _conection;

        public AttributesType_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<AttributesTypes_DTOS>> LISTAR_ATTRIBUTESTYPE_ASYNC()
        {
            var list = new List<AttributesTypes_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_AttributesTypes_List]", con))
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

        public async Task<IEnumerable<AttributesTypes_DTOS>> FILTRAR_ATTRIBUTESTYPE_ASYNC(
            int? attributeTypeId,
            string attributeTypeName,
            string attributeTypeDescription,
            int? attributeTypeCreatorId,
            DateTime? attributeTypeCreationDate,
            int? attributeTypeModificatorId,
            DateTime? attributeTypeModificationDate,
            bool? attributeTypeStatusId)
        {
            var list = new List<AttributesTypes_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_AttributesTypes_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@attributeTypeId", attributeTypeId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeName", attributeTypeName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeDescription", attributeTypeDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeCreatorId", attributeTypeCreatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeCreationDate", attributeTypeCreationDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeModificatorId", attributeTypeModificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeModificationDate", attributeTypeModificationDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeStatusId", attributeTypeStatusId ?? (object)DBNull.Value));

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

        public async Task<(int code, string message, int? templateId)> NUEVO_ATTRIBUTESTYPE_ASYNC(
            string attributeTypeName,
            string attributeTypeDescription,
            int attributeTypeCreatorId,
            int? attributeTypeModificatorId,
            bool attributeTypeStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_AttributesTypes_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@attributeTypeName", attributeTypeName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeDescription", attributeTypeDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeCreatorId", attributeTypeCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeModificatorId", attributeTypeModificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeStatusId", attributeTypeStatusId));

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
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el tipo de atributo", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_ATTRIBUTESTYPE_ASYNC(
            int attributeTypeId,
            string attributeTypeName,
            string attributeTypeDescription,
            int attributeTypeCreatorId,
            int attributeTypeModificatorId,
            bool attributeTypeStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_AttributesTypes_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@attributeTypeId", attributeTypeId));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeName", attributeTypeName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeDescription", attributeTypeDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeCreatorId", attributeTypeCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeModificatorId", attributeTypeModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeStatusId", attributeTypeStatusId));

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
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el tipo de atributo", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_ATTRIBUTESTYPE_ASYNC(
            int attributeTypeId,
            string attributeTypeName,
            string attributeTypeDescription,
            int attributeTypeCreatorId,
            int attributeTypeModificatorId,
            bool attributeTypeStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_AttributesTypes_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@attributeTypeId", attributeTypeId));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeName", attributeTypeName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeDescription", attributeTypeDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeCreatorId", attributeTypeCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeModificatorId", attributeTypeModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@attributeTypeStatusId", attributeTypeStatusId));

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
            catch (Exception ex) { throw new Exception("Error al eliminar el tipo de atributo", ex); }
        }

        private AttributesTypes_DTOS MapearDTO(SqlDataReader dr)
        {
            return new AttributesTypes_DTOS
            {
                AttributeTypeId = dr["attributeTypeId"] as int?,
                AttributeTypeName = dr["attributeTypeName"]?.ToString() ?? string.Empty,
                AttributeTypeDescription = dr["attributeTypeDescription"]?.ToString() ?? string.Empty,
                AttributeTypeCreatorId = dr["attributeTypeCreatorId"] as int?,
                AttributeTypeCreationDate = dr["attributeTypeCreationDate"] as DateTime?,
                AttributeTypeModificatorId = dr["attributeTypeModificatorId"] as int?,
                AttributeTypeModificationDate = dr["attributeTypeModificationDate"] as DateTime?,
                AttributeTypeStatusId = dr["attributeTypeStatusId"] as bool?
            };
        }
    }
}