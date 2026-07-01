using Ecom_Aplication.Dtos;
using Ecom_Aplication.Interfaces;
using Ecom_Infraestructure.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Ecom_Infraestructure.Repository
{
    public class UserPaymentMethods_Repository : IUserPaymentMethods
    {
        private readonly DB_conection _conection;

        public UserPaymentMethods_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<UserPaymentMethods_DTOS>> LISTAR_USERPAYMENTMETHODS_ASYNC()
        {
            var list = new List<UserPaymentMethods_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_UserPaymentMethods_List]", con))
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

        public async Task<IEnumerable<UserPaymentMethods_DTOS>> FILTRAR_USERPAYMENTMETHODS_ASYNC(string searchTerm, bool? statusId)
        {
            var list = new List<UserPaymentMethods_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_UserPaymentMethods_Filter]", con))
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

        public async Task<(int code, string message, int? templateId)> NUEVO_USERPAYMENTMETHODS_ASYNC(
            int userPaymentMethodUserId,
            int userPaymentMethodPaymentMethodTypeId,
            string cardNumberPlain,
            string expirationDatePlain,
            string cvvPlain,
            string userPaymentMethodCardHolderName,
            int userPaymentMethodCreatorId,
            bool userPaymentMethodStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_UserPaymentMethods_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userPaymentMethodUserId", userPaymentMethodUserId));
                    cmd.Parameters.Add(new SqlParameter("@userPaymentMethodPaymentMethodTypeId", userPaymentMethodPaymentMethodTypeId));
                    cmd.Parameters.Add(new SqlParameter("@CardNumberPlain", cardNumberPlain ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@ExpirationDatePlain", expirationDatePlain ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@CVVPlain", cvvPlain ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userPaymentMethodCardHolderName", userPaymentMethodCardHolderName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userPaymentMethodCreatorId", userPaymentMethodCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@userPaymentMethodStatusId", userPaymentMethodStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Método de pago registrado correctamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el método de pago", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ACTUALIZAR_USERPAYMENTMETHODS_ASYNC(
            int userPaymentMethodId,
            int userPaymentMethodPaymentMethodTypeId,
            string cardNumberPlain,
            string expirationDatePlain,
            string cvvPlain,
            string userPaymentMethodCardHolderName,
            int userPaymentMethodModificatorId,
            bool userPaymentMethodStatusId,
            bool forzarRecuperacion)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_UserPaymentMethods_Update]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@userPaymentMethodId", userPaymentMethodId));
                    cmd.Parameters.Add(new SqlParameter("@userPaymentMethodPaymentMethodTypeId", userPaymentMethodPaymentMethodTypeId));
                    cmd.Parameters.Add(new SqlParameter("@CardNumberPlain", cardNumberPlain ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@ExpirationDatePlain", expirationDatePlain ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@CVVPlain", cvvPlain ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userPaymentMethodCardHolderName", userPaymentMethodCardHolderName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@userPaymentMethodModificatorId", userPaymentMethodModificatorId));
                    cmd.Parameters.Add(new SqlParameter("@userPaymentMethodStatusId", userPaymentMethodStatusId));
                    cmd.Parameters.Add(new SqlParameter("@ForzarRecuperacion", forzarRecuperacion));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Método de pago actualizado correctamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al actualizar el método de pago", ex); }
        }

        public async Task<(int code, string message, int? templateId)> ELIMINAR_USERPAYMENTMETHODS_ASYNC(
            int userPaymentMethodId,
            int userPaymentMethodModificatorId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_UserPaymentMethods_Delete]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@userPaymentMethodId", userPaymentMethodId));
                    cmd.Parameters.Add(new SqlParameter("@userPaymentMethodModificatorId", userPaymentMethodModificatorId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Método de pago inactivado (eliminado) correctamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error al eliminar el método de pago", ex); }
        }

        // ADVERTENCIA: List/Filter devuelven texto plano desencriptado (cardNumberDecrypted, etc.),
        // pero el DTO define estos campos como byte[]?. Aquí se convierte el string a bytes UTF8
        // únicamente para poder llenar la propiedad; estos bytes NO corresponden al valor encriptado
        // real de la columna en SQL Server y no se pueden volver a desencriptar con la llave simétrica.
        private UserPaymentMethods_DTOS MapearDTO(SqlDataReader dr)
        {
            return new UserPaymentMethods_DTOS
            {
                userPaymentMethodId = dr["userPaymentMethodId"] as int?,
                userPaymentMethodUserId = HasColumn(dr, "userId") ? dr["userId"] as int? : null,
                userPaymentMethodPaymentMethodTypeId = HasColumn(dr, "paymentMethodTypeId") ? dr["paymentMethodTypeId"] as int? : null,
                userPaymentMethodCardNumber = ToBytesOrNull(dr, "cardNumberDecrypted"),
                userPaymentMethodExpirationDate = ToBytesOrNull(dr, "expirationDateDecrypted"),
                userPaymentMethodCVV = ToBytesOrNull(dr, "cvvDecrypted"),
                userPaymentMethodCardHolderName = HasColumn(dr, "cardHolderName") ? dr["cardHolderName"]?.ToString() : string.Empty,
                userPaymentMethodCreatorId = null,   
                userPaymentMethodCreationDate = null, 
                userPaymentMethodModificatorId = null, 
                userPaymentMethodModificationDate = null, 
                userPaymentMethodStatusId = HasColumn(dr, "statusId") ? dr["statusId"] as bool? : null
            };
        }

        private byte[]? ToBytesOrNull(SqlDataReader dr, string columnName)
        {
            if (!HasColumn(dr, columnName) || dr[columnName] == DBNull.Value)
                return null;

            string value = dr[columnName].ToString() ?? string.Empty;
            return Encoding.UTF8.GetBytes(value);
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