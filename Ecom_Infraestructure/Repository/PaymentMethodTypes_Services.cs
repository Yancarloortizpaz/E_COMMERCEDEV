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
    public class PaymentMethodTypes_Repository : IPaymentMethodTypes
    {
        private readonly DB_conection _conection;

        public PaymentMethodTypes_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<PaymentMethodTypes_DTOS>> LISTAR_PAYMENTMETHODTYPES_ASYNC()
        {
            var list = new List<PaymentMethodTypes_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_PaymentMethodTypes_List]", con))
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

        public async Task<IEnumerable<PaymentMethodTypes_DTOS>> FILTRAR_PAYMENTMETHODTYPES_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<PaymentMethodTypes_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_PaymentMethodTypes_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@SearchTerm", searchTerm ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@StatusId", statusId ?? (object)DBNull.Value));

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

        public async Task<(int code, string message)> NUEVO_PAYMENTMETHODTYPES_ASYNC(
            string paymentMethodTypeName,
            string paymentMethodTypeDescription,
            int paymentMethodTypeCreatorId,
            bool paymentMethodTypeStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_PaymentMethodTypes_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeName", paymentMethodTypeName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeDescription", paymentMethodTypeDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeCreatorId", paymentMethodTypeCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeStatusId", paymentMethodTypeStatusId));

                    int rows = await cmd.ExecuteNonQueryAsync();

                    return rows > 0
                        ? (200, "Tipo de método de pago creado exitosamente.")
                        : (-1, "No se pudo crear el tipo de método de pago.");
                }
            }
            catch (SqlException ex)
            {
                return (ex.Number, ex.Message);
            }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el tipo de método de pago", ex); }
        }

        public async Task<(int code, string message)> ACTUALIZAR_PAYMENTMETHODTYPES_ASYNC(
            int paymentMethodTypeId,
            string paymentMethodTypeName,
            string paymentMethodTypeDescription,
            int paymentMethodTypeModificatorId,
            bool paymentMethodTypeStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_PaymentMethodTypes_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeId", paymentMethodTypeId));
                    cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeName", paymentMethodTypeName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeDescription", paymentMethodTypeDescription ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeModificatorId", paymentMethodTypeModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeStatusId", paymentMethodTypeStatusId));

                    int rows = await cmd.ExecuteNonQueryAsync();

                    return rows > 0
                        ? (200, "Tipo de método de pago actualizado exitosamente.")
                        : (-1, "No se encontró el tipo de método de pago a actualizar.");
                }
            }
            catch (SqlException ex)
            {
                return (ex.Number, ex.Message);
            }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el tipo de método de pago", ex); }
        }

        public async Task<(int code, string message)> ELIMINAR_PAYMENTMETHODTYPES_ASYNC(
            int paymentMethodTypeId,
            int paymentMethodTypeModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_CATALOGS].[sp_PaymentMethodTypes_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeId", paymentMethodTypeId));
                    cmd.Parameters.Add(new SqlParameter("@paymentMethodTypeModificatorId", paymentMethodTypeModificatorId));

                    int rows = await cmd.ExecuteNonQueryAsync();

                    return rows > 0
                        ? (200, "Tipo de método de pago eliminado (inactivado) exitosamente.")
                        : (-1, "No se encontró el tipo de método de pago a eliminar.");
                }
            }
            catch (SqlException ex)
            {
                return (ex.Number, ex.Message);
            }
            catch (Exception ex) { throw new Exception("Error al eliminar el tipo de método de pago", ex); }
        }

        private PaymentMethodTypes_DTOS MapearDTO(SqlDataReader dr)
        {
            return new PaymentMethodTypes_DTOS
            {
                paymentMethodTypeId = dr["paymentMethodTypeId"] as int?,
                paymentMethodTypeName = dr["paymentMethodTypeName"]?.ToString() ?? string.Empty,
                paymentMethodTypeDescription = dr["paymentMethodTypeDescription"]?.ToString() ?? string.Empty,
                paymentMethodTypeCreatorId = HasColumn(dr, "paymentMethodTypeCreatorId") ? dr["paymentMethodTypeCreatorId"] as int? : null,
                paymentMethodTypeCreationDate = HasColumn(dr, "paymentMethodTypeCreationDate") ? dr["paymentMethodTypeCreationDate"] as DateTime? : null,
                paymentMethodTypeModificatorId = HasColumn(dr, "paymentMethodTypeModificatorId") ? dr["paymentMethodTypeModificatorId"] as int? : null,
                paymentMethodTypeModificationDate = HasColumn(dr, "paymentMethodTypeModificationDate") ? dr["paymentMethodTypeModificationDate"] as DateTime? : null,
                paymentMethodTypeStatusId = dr["paymentMethodTypeStatusId"] as bool?
            };
        }

        private bool HasColumn(SqlDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}