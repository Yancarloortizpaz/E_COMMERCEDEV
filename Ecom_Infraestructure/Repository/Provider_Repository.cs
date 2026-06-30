using Ecom_Aplication.Interfaces;
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
    public class Provider_Repository : IProviderRepository
    {
        private readonly DB_conection _conection;

        public Provider_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<Providers>> LISTAR_PROVIDER_ASYNC()
        {
            var list = new List<Providers>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Providers_List]", con))
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

        public async Task<IEnumerable<Providers>> FILTRAR_PROVIDER_ASYNC(string searchTerm, int? providerId)
        {
            var list = new List<Providers>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Providers_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@searchTerm", searchTerm ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@providerId", providerId ?? (object)DBNull.Value));

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

        public async Task<(int code, string message, int? templateId)> NUEVO_PROVIDER_ASYNC(
            string providerName,
            string providerDescription,
            int providerCreatorId,
            bool providerStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Providers_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@providerName", providerName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@providerDescription", providerDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@providerCreatorId", providerCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@providerStatusId", providerStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Proveedor creado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el proveedor", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_PROVIDER_ASYNC(
            int providerId,
            string providerName,
            string providerDescription,
            int providerModificatorId,
            bool providerStatusId,
            bool forzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Providers_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@providerId", providerId));
                    cmd.Parameters.Add(new SqlParameter("@providerName", providerName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@providerDescription", providerDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@providerModificatorId", providerModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@providerStatusId", providerStatusId));
                    cmd.Parameters.Add(new SqlParameter("@ForzarRecuperacion", forzarRecuperacion));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Proveedor actualizado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el proveedor", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_PROVIDER_ASYNC(int providerId, int providerModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Providers_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@providerId", providerId));
                    cmd.Parameters.Add(new SqlParameter("@providerModificatorId", providerModificatorId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Proveedor eliminado exitosamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el proveedor", ex); }
        }

        private Providers MapearEntidadDominio(SqlDataReader dr)
        {
            return new Providers
            {
                ProviderId = dr["providerId"] as int?,
                ProviderName = dr["providerName"]?.ToString() ?? string.Empty,
                ProviderDescription = dr["providerDescription"]?.ToString() ?? string.Empty,
                ProviderCreatorId = dr["providerCreatorId"] as int?,
                ProviderCreationDate = dr["providerCreationDate"] as DateTime?,
                ProviderModificatorId = dr["providerModificatorId"] as int?,
                ProviderModificationDate = dr["providerModificationDate"] as DateTime?,
                ProviderStatusId = dr["providerStatusId"] as bool?
            };
        }
    }
}
