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
    public class AttributeProductVariables_Repository : IAttributeProductVariablesRepository
    {
        private readonly DB_conection _conection;

        public AttributeProductVariables_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<AttributeProductVariables>> OBTENER_POR_PRODUCTVARIABLE_ATTRIBUTEPRODUCTVARIABLES_ASYNC(int ProductVariableId)
        {
            var list = new List<AttributeProductVariables>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_AttributeProductVariables_GetByProductVariable]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ProductVariableId", ProductVariableId));

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

        public async Task<(int code, string message, int? templateId)> NUEVO_ATTRIBUTEPRODUCTVARIABLES_ASYNC(
            int attributeProductVariableProductVariableId,
            int attributeProductVariableAttributeProductId,
            string attributeProductVariableValue,
            int attributeProductVariableCreatorId,
            bool attributeProductVariableStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_AttributeProductVariables_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableProductVariableId", attributeProductVariableProductVariableId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableAttributeProductId", attributeProductVariableAttributeProductId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableValue", attributeProductVariableValue ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableCreatorId", attributeProductVariableCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableStatusId", attributeProductVariableStatusId));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Variable de atributo creada con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar variable de atributo", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_ATTRIBUTEPRODUCTVARIABLES_ASYNC(
            int attributeProductVariableId,
            int attributeProductVariableProductVariableId,
            int attributeProductVariableAttributeProductId,
            string attributeProductVariableValue,
            int attributeProductVariableModificatorId,
            bool attributeProductVariableStatusId,
            bool ForzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_AttributeProductVariables_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableId", attributeProductVariableId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableProductVariableId", attributeProductVariableProductVariableId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableAttributeProductId", attributeProductVariableAttributeProductId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableValue", attributeProductVariableValue ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableModificatorId", attributeProductVariableModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableStatusId", attributeProductVariableStatusId));
                    cmd.Parameters.Add(new SqlParameter("@ForzarRecuperacion", ForzarRecuperacion));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Variable de atributo actualizada con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar variable de atributo", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_ATTRIBUTEPRODUCTVARIABLES_ASYNC(int attributeProductVariableId, int attributeProductVariableModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_AttributeProductVariables_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableId", attributeProductVariableId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableModificatorId", attributeProductVariableModificatorId));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Variable de atributo eliminada con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar la variable de atributo", ex); }
        }

        private AttributeProductVariables MapearEntidadDominio(SqlDataReader dr)
        {
            return new AttributeProductVariables
            {
                attributeProductVariableId = dr["AttributeProductVariableId"] as int?,
                attributeProductVariableProductVariableId = dr["AttributeProductVariableProductVariableId"] as int?,
                attributeProductVariableAttributeProductId = dr["AttributeProductVariableAttributeProductId"] as int?,
                attributeProductVariableValue = dr["AttributeProductVariableValue"]?.ToString() ?? string.Empty,
                attributeProductVariableCreatorId = dr["AttributeProductVariableCreatorId"] as int?,
                attributeProductVariableCreationDate = dr["AttributeProductVariableCreationDate"] as DateTime?,
                attributeProductVariableModificatorId = dr["AttributeProductVariableModificatorId"] as int?,
                attributeProductVariableModificationDate = dr["AttributeProductVariableModificationDate"] as DateTime?,
                attributeProductVariableStatusId = dr["AttributeProductVariableStatusId"] as bool?
            };
        }
    }
}
