using APLICATION.Interfaces;
using DOMAIN.AttributeProductVariables;
using DOMAIN.VariablesSalida;
using INFRASTRUCTURE.DB;
using Microsoft.Data.SqlClient;
using System.Data;

namespace INFRASTRUCTURE.Repository
{
    public class AttributeProductVariablesRepository : IAttributeProductVariablesRepository
    {
        private readonly DBconexionfactory _connection;

        public AttributeProductVariablesRepository(DBconexionfactory connection)
        {
            _connection = connection;
        }

        #region escritura_attributeproductvariables

        public async Task<OUTPUT> Insertar_AttributeProductVariablesAsync(DM_AttributeProductVariablesInsertar modelo)
        {
            var result = new OUTPUT();
            try
            {
                using var con = _connection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_AttributeProductVariables_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableProductVariableId", modelo.AttributeProductVariableProductVariableId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableAttributeProductId", modelo.AttributeProductVariableAttributeProductId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableValue", modelo.AttributeProductVariableValue ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableCreatorId", modelo.AttributeProductVariableCreatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableStatusId", modelo.AttributeProductVariableStatusId ?? (object)DBNull.Value));

                    SqlParameter pCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter pMessage = new SqlParameter("@o_message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
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
                throw new Exception($"Error en el motor SQL al crear la variable de atributo de producto: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error crítico de infraestructura al crear la variable de atributo de producto: {ex.Message}", ex);
            }
        }

        public async Task<OUTPUT> Editar_AttributeProductVariablesAsync(DM_AttributeProductVariablesEditar modelo)
        {
            var result = new OUTPUT();
            try
            {
                using var con = _connection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_AttributeProductVariables_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableId", modelo.AttributeProductVariableId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableProductVariableId", modelo.AttributeProductVariableProductVariableId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableAttributeProductId", modelo.AttributeProductVariableAttributeProductId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableValue", modelo.AttributeProductVariableValue ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableModificatorId", modelo.AttributeProductVariableModificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableStatusId", modelo.AttributeProductVariableStatusId ?? (object)DBNull.Value));

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
                throw new Exception($"Error en el motor SQL al actualizar la variable de atributo de producto: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error crítico de infraestructura al actualizar la variable de atributo de producto: {ex.Message}", ex);
            }
        }

        public async Task<OUTPUT> Eliminar_AttributeProductVariablesAsync(int? attributeProductVariableId, int? attributeProductVariableModificatorId)
        {
            var result = new OUTPUT();
            try
            {
                using var con = _connection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_AttributeProductVariables_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableId", attributeProductVariableId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableModificatorId", attributeProductVariableModificatorId ?? (object)DBNull.Value));

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
                throw new Exception($"Error en el motor SQL al eliminar la variable de atributo de producto: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error crítico de infraestructura al eliminar la variable de atributo de producto: {ex.Message}", ex);
            }
        }

        #endregion

        #region lectura_attributeproductvariables

        public async Task<IEnumerable<DM_AttributeProductVariablesListar>> Listar_AttributeProductVariablesAsync()
        {
            var list = new List<DM_AttributeProductVariablesListar>();
            try
            {
                using var con = _connection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_AttributeProductVariables_List]", con))
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
                throw new Exception($"Error al consultar el listado completo de variables de atributos de productos: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error crítico al consultar el listado completo de variables de atributos de productos: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<DM_AttributeProductVariablesFiltrar>> Filtrar_AttributeProductVariablesAsync(DM_AttributeProductVariablesFiltrar filtro)
        {
            var list = new List<DM_AttributeProductVariablesFiltrar>();
            try
            {
                using var con = _connection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_AttributeProductVariables_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableId", filtro.AttributeProductVariableId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableProductVariableId", filtro.AttributeProductVariableProductVariableId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableAttributeProductId", filtro.AttributeProductVariableAttributeProductId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableValue", filtro.AttributeProductVariableValue ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableCreatorId", filtro.AttributeProductVariableCreatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableCreationDate", filtro.AttributeProductVariableCreationDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableModificatorId", filtro.AttributeProductVariableModificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableModificationDate", filtro.AttributeProductVariableModificationDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductVariableStatusId", filtro.AttributeProductVariableStatusId ?? (object)DBNull.Value));

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
                throw new Exception($"Error al filtrar variables de atributos de productos en la base de datos: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error crítico al filtrar variables de atributos de productos: {ex.Message}", ex);
            }
        }

        #endregion

        #region mapeadores

        private int? ObtenerInt(SqlDataReader dr, string column)
        {
            try
            {
                int ordinal = dr.GetOrdinal(column);
                if (dr.IsDBNull(ordinal)) return null;
                object val = dr.GetValue(ordinal);
                if (val is bool b) return b ? 1 : 0;
                return Convert.ToInt32(val);
            }
            catch
            {
                return null;
            }
        }

        private bool? ObtenerBool(SqlDataReader dr, string column)
        {
            try
            {
                int ordinal = dr.GetOrdinal(column);
                if (dr.IsDBNull(ordinal)) return null;
                object val = dr.GetValue(ordinal);
                if (val is bool b) return b;
                return Convert.ToBoolean(val);
            }
            catch
            {
                return null;
            }
        }

        private string? ObtenerString(SqlDataReader dr, string column)
        {
            try
            {
                int ordinal = dr.GetOrdinal(column);
                if (dr.IsDBNull(ordinal)) return null;
                return dr.GetValue(ordinal)?.ToString();
            }
            catch
            {
                return null;
            }
        }

        private DateTime? ObtenerDateTime(SqlDataReader dr, string column)
        {
            try
            {
                int ordinal = dr.GetOrdinal(column);
                if (dr.IsDBNull(ordinal)) return null;
                return Convert.ToDateTime(dr.GetValue(ordinal));
            }
            catch
            {
                return null;
            }
        }

        private DM_AttributeProductVariablesListar MapearDataReaderAListar(SqlDataReader dr)
        {
            return new DM_AttributeProductVariablesListar
            {
                AttributeProductVariableId = ObtenerInt(dr, "attributeProductVariableId"),
                AttributeProductVariableProductVariableId = ObtenerInt(dr, "attributeProductVariableProductVariableId"),
                AttributeProductVariableAttributeProductId = ObtenerInt(dr, "attributeProductVariableAttributeProductId"),
                AttributeProductVariableValue = ObtenerString(dr, "attributeProductVariableValue"),
                AttributeProductVariableCreatorId = ObtenerInt(dr, "attributeProductVariableCreatorId"),
                AttributeProductVariableStatusId = ObtenerBool(dr, "attributeProductVariableStatusId")
            };
        }

        private DM_AttributeProductVariablesFiltrar MapearDataReaderAFiltrar(SqlDataReader dr)
        {
            return new DM_AttributeProductVariablesFiltrar
            {
                AttributeProductVariableId = ObtenerInt(dr, "attributeProductVariableId"),
                AttributeProductVariableProductVariableId = ObtenerInt(dr, "attributeProductVariableProductVariableId"),
                AttributeProductVariableAttributeProductId = ObtenerInt(dr, "attributeProductVariableAttributeProductId"),
                AttributeProductVariableValue = ObtenerString(dr, "attributeProductVariableValue"),
                AttributeProductVariableCreatorId = ObtenerInt(dr, "attributeProductVariableCreatorId"),
                AttributeProductVariableCreationDate = ObtenerDateTime(dr, "attributeProductVariableCreationDate"),
                AttributeProductVariableModificatorId = ObtenerInt(dr, "attributeProductVariableModificatorId"),
                AttributeProductVariableModificationDate = ObtenerDateTime(dr, "attributeProductVariableModificationDate"),
                AttributeProductVariableStatusId = ObtenerBool(dr, "attributeProductVariableStatusId")
            };
        }

        #endregion
    }
}
