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
    public class AttributeProducts_Repository : IAttributeProductsRepository
    {
        private readonly DB_conection _conection;

        public AttributeProducts_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<AttributeProducts>> LISTAR_ATTRIBUTEPRODUCTS_ASYNC()
        {
            var list = new List<AttributeProducts>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_AttributeProducts_List]", con))
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

        public async Task<IEnumerable<AttributeProducts>> FILTRAR_ATTRIBUTEPRODUCTS_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<AttributeProducts>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_AttributeProducts_Filter]", con))
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

        public async Task<(int code, string message, int? templateId)> NUEVO_ATTRIBUTEPRODUCTS_ASYNC(
            int attributeProductAttributesTypeId,
            string attributeProductName,
            string attributeProductDescription,
            int attributeProductCreatorId,
            bool attributeProductStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_AttributeProducts_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@attributeProductAttributesTypeId", attributeProductAttributesTypeId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductName", attributeProductName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductDescription", attributeProductDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductCreatorId", attributeProductCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductStatusId", attributeProductStatusId));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Atributo de producto creado con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el atributo", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_ATTRIBUTEPRODUCTS_ASYNC(
            int attributeProductId,
            int attributeProductAttributesTypeId,
            string attributeProductName,
            string attributeProductDescription,
            int attributeProductModificatorId,
            bool attributeProductStatusId,
            bool forzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_AttributeProducts_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@attributeProductId", attributeProductId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductAttributesTypeId", attributeProductAttributesTypeId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductName", attributeProductName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductDescription", attributeProductDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductModificatorId", attributeProductModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductStatusId", attributeProductStatusId));
                    cmd.Parameters.Add(new SqlParameter("@forzarRecuperacion", forzarRecuperacion));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Atributo de producto actualizado con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el atributo", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_ATTRIBUTEPRODUCTS_ASYNC(int attributeProductId, int attributeProductModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_AttributeProducts_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@attributeProductId", attributeProductId));
                    cmd.Parameters.Add(new SqlParameter("@attributeProductModificatorId", attributeProductModificatorId));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Atributo de producto eliminado con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el atributo", ex); }
        }

        private AttributeProducts MapearEntidadDominio(SqlDataReader dr)
        {
            return new AttributeProducts
            {
                AttributeProductId = dr["AttributeProductId"] as int?,
                AttributeProductAttributesTypeId = dr["AttributeProductAttributesTypeId"] as int?,
                AttributeProductName = dr["AttributeProductName"]?.ToString() ?? string.Empty,
                AttributeProductDescription = dr["AttributeProductDescription"]?.ToString() ?? string.Empty,
                AttributeProductCreatorId = dr["AttributeProductCreatorId"] as int?,
                AttributeProductCreationDate = dr["AttributeProductCreationDate"] as DateTime?,
                AttributeProductModificatorId = dr["AttributeProductModificatorId"] as int?,
                AttributeProductModificationDate = dr["AttributeProductModificationDate"] as DateTime?,
                AttributeProductStatusId = dr["AttributeProductStatusId"] as bool?
            };
        }
    }
}