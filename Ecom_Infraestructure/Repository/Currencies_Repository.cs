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
    public class Currencies_Repository : ICurrenciesRepository
    {
        private readonly DB_conection _conection;

        public Currencies_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<Currencies>> LISTAR_CURRENCIES_ASYNC()
        {
            var list = new List<Currencies>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Currencies_List]", con))
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

        public async Task<IEnumerable<Currencies>> FILTRAR_CURRENCIES_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<Currencies>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Currencies_Filter]", con))
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

        public async Task<(int code, string message, int? templateId)> NUEVO_CURRENCIES_ASYNC(
            string currencyName,
            string currencyISO,
            int currencyCode,
            string currencyDescription,
            int currencyCreatorId,
            bool currencyStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Currencies_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@currencyName", currencyName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@currencyISO", currencyISO ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@currencyCode", currencyCode));
                    cmd.Parameters.Add(new SqlParameter("@currencyDescription", currencyDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@currencyCreatorId", currencyCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@currencyStatusId", currencyStatusId));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Moneda creada con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar la moneda", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_CURRENCIES_ASYNC(
            int currencyId,
            string currencyName,
            string currencyISO,
            int currencyCode,
            string currencyDescription,
            int currencyModificatorId,
            bool currencyStatusId,
            bool ForzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Currencies_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@currencyId", currencyId));
                    cmd.Parameters.Add(new SqlParameter("@currencyName", currencyName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@currencyISO", currencyISO ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@currencyCode", currencyCode));
                    cmd.Parameters.Add(new SqlParameter("@currencyDescription", currencyDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@currencyModificatorId", currencyModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@currencyStatusId", currencyStatusId));
                    cmd.Parameters.Add(new SqlParameter("@ForzarRecuperacion", ForzarRecuperacion));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Moneda actualizada con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar la moneda", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_CURRENCIES_ASYNC(int currencyId, int currencyModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_Currencies_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@currencyId", currencyId));
                    cmd.Parameters.Add(new SqlParameter("@currencyModificatorId", currencyModificatorId));

                    await cmd.ExecuteNonQueryAsync();
                    return (200, "Moneda eliminada con éxito", null);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar la moneda", ex); }
        }

        private Currencies MapearEntidadDominio(SqlDataReader dr)
        {
            return new Currencies
            {
                currencyId = dr["CurrencyId"] as int?,
                currencyName = dr["CurrencyName"]?.ToString() ?? string.Empty,
                currencyISO = dr["CurrencyISO"]?.ToString() ?? string.Empty,
                currencyCode = dr["CurrencyCode"] as int?,
                currencyDescription = dr["CurrencyDescription"]?.ToString() ?? string.Empty,
                currencyCreatorId = dr["CurrencyCreatorId"] as int? ?? 0,
                currencyCreationDate = dr["CurrencyCreationDate"] as DateTime?,
                currencyModificatorId = dr["CurrencyModificatorId"] as int?,
                currencyModificationDate = dr["CurrencyModificationDate"] as DateTime?,
                currencyStatusId = dr["CurrencyStatusId"] as bool?
            };
        }
    }
}
