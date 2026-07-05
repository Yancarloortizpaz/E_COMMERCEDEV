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
    public class MarkByProviders_Repository : IMarkByProviders
    {
        private readonly DB_conection _conection;

        public MarkByProviders_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<MarkByProviders>> LISTAR_MARKBYPROVIDERS_ASYNC()
        {
            var list = new List<MarkByProviders>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_MarkByProviders_List]", con))
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

        public async Task<IEnumerable<MarkByProviders>> FILTRAR_MARKBYPROVIDERS_ASYNC(
            int? markByProviderId,
            int? markByProviderMarkId,
            int? markByProviderProviderId,
            int? markByProviderCreatorId,
            DateTime? markByProviderCreationDate,
            int? markByProviderModificatorId,
            DateTime? markByProviderModificationDate,
            bool? markByProviderStatusId)
        {
            var list = new List<MarkByProviders>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_MarkByProviders_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@markByProviderId", markByProviderId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderMarkId", markByProviderMarkId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderProviderId", markByProviderProviderId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderCreatorId", markByProviderCreatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderCreationDate", markByProviderCreationDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderModificatorId", markByProviderModificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderModificationDate", markByProviderModificationDate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderStatusId", markByProviderStatusId ?? (object)DBNull.Value));

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

        public async Task<(int code, string message, int? templateId)> NUEVO_MARKBYPROVIDERS_ASYNC(
            int markByProviderMarkId,
            int markByProviderProviderId,
            int markByProviderCreatorId,
            int? markByProviderModificatorId,
            bool markByProviderStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_MarkByProviders_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@markByProviderMarkId", markByProviderMarkId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderProviderId", markByProviderProviderId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderCreatorId", markByProviderCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderModificatorId", markByProviderModificatorId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderStatusId", markByProviderStatusId));

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
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar la relación marca-proveedor", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_MARKBYPROVIDERS_ASYNC(
            int markByProviderId,
            int markByProviderMarkId,
            int markByProviderProviderId,
            int markByProviderCreatorId,
            int markByProviderModificatorId,
            bool markByProviderStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_MarkByProviders_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@markByProviderId", markByProviderId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderMarkId", markByProviderMarkId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderProviderId", markByProviderProviderId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderCreatorId", markByProviderCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderModificatorId", markByProviderModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderStatusId", markByProviderStatusId));

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
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar la relación marca-proveedor", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_MARKBYPROVIDERS_ASYNC(
            int markByProviderId,
            int markByProviderMarkId,
            int markByProviderProviderId,
            int markByProviderCreatorId,
            int markByProviderModificatorId,
            bool markByProviderStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_MarkByProviders_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@markByProviderId", markByProviderId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderMarkId", markByProviderMarkId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderProviderId", markByProviderProviderId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderCreatorId", markByProviderCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderModificatorId", markByProviderModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@markByProviderStatusId", markByProviderStatusId));

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
            catch (Exception ex) { throw new Exception("Error al eliminar la relación marca-proveedor", ex); }
        }

        private MarkByProviders MapearEntidadDominio(SqlDataReader dr)
        {
          
            object? GetValueSafe(string columnName)
            {
                try
                {
                    int ordinal = dr.GetOrdinal(columnName);
                    return dr.IsDBNull(ordinal) ? null : dr.GetValue(ordinal);
                }
                catch (IndexOutOfRangeException)
                {
                    return null; 
                }
            }

       
            return new MarkByProviders
            {
                MarkByProviderId = GetValueSafe("markByProviderId") as int?,
                MarkByProviderMarkId = GetValueSafe("markByProviderMarkId") as int?,
                MarkByProviderProviderId = GetValueSafe("markByProviderProviderId") as int?,
                MarkByProviderCreatorId = GetValueSafe("markByProviderCreatorId") as int?,
                MarkByProviderCreationDate = GetValueSafe("markByProviderCreationDate") as DateTime?,
                MarkByProviderModificatorId = GetValueSafe("markByProviderModificatorId") as int?,
                MarkByProviderModificationDate = GetValueSafe("markByProviderModificationDate") as DateTime?,
                MarkByProviderStatusId = GetValueSafe("markByProviderStatusId") as bool?
            };
        }
    }
    }

