using Ecom_Aplication.Dtos;
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
    public class PaymentOrderDetails_Repository : IPaymentOrderDetails
    {
        private readonly DB_conection _conection;

        public PaymentOrderDetails_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<PaymentOrderDetails_DTOS>> LISTAR_PAYMENTORDERDETAILS_ASYNC()
        {
            var list = new List<PaymentOrderDetails_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_PaymentOrderDetails_List]", con))
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

        public async Task<IEnumerable<PaymentOrderDetails_DTOS>> FILTRAR_PAYMENTORDERDETAILS_ASYNC(int? orderId, string searchTerm)
        {
            var list = new List<PaymentOrderDetails_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_PaymentOrderDetails_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@OrderId", orderId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@SearchTerm", searchTerm ?? (object)DBNull.Value));

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

        public async Task<(int code, string message, int? templateId)> NUEVO_PAYMENTORDERDETAILS_ASYNC(
            int orderDetailOrderId,
            int orderDetailProductVariableId,
            decimal orderDetailPrice,
            int orderDetailQuantity,
            decimal orderDetailDiscount,
            decimal orderDetailSubTotal,
            decimal orderDetailTAX,
            decimal orderDetailTotal,
            int orderDetailCurrencyId,
            int orderDetailCreatorId,
            bool orderDetailStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_PaymentOrderDetails_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@orderDetailOrderId", orderDetailOrderId));
                    cmd.Parameters.Add(new SqlParameter("@orderDetailProductVariableId", orderDetailProductVariableId));
                    cmd.Parameters.Add(new SqlParameter("@orderDetailPrice", orderDetailPrice));
                    cmd.Parameters.Add(new SqlParameter("@orderDetailQuantity", orderDetailQuantity));
                    cmd.Parameters.Add(new SqlParameter("@orderDetailDiscount", orderDetailDiscount));
                    cmd.Parameters.Add(new SqlParameter("@orderDetailSubTotal", orderDetailSubTotal));
                    cmd.Parameters.Add(new SqlParameter("@orderDetailTAX", orderDetailTAX));
                    cmd.Parameters.Add(new SqlParameter("@orderDetailTotal", orderDetailTotal));
                    cmd.Parameters.Add(new SqlParameter("@orderDetailCurrencyId", orderDetailCurrencyId));
                    cmd.Parameters.Add(new SqlParameter("@orderDetailCreatorId", orderDetailCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@orderDetailStatusId", orderDetailStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Detalle de orden de pago registrado correctamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al insertar el detalle de orden de pago", ex); }
        }

        private PaymentOrderDetails_DTOS MapearDTO(SqlDataReader dr)
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

            return new PaymentOrderDetails_DTOS
            {
                orderDetailId = GetValueSafe("orderDetailId") as int?,
                orderDetailOrderId = GetValueSafe("orderId") as int?,
                orderDetailProductVariableId = GetValueSafe("productVariableId") as int?,
                orderDetailPrice = GetValueSafe("orderDetailPrice") as decimal?, 
                orderDetailQuantity = GetValueSafe("orderDetailQuantity") as int?,
                orderDetailDiscount = GetValueSafe("orderDetailDiscount") as decimal?,
                orderDetailSubTotal = GetValueSafe("orderDetailSubTotal") as decimal?,
                orderDetailTAX = GetValueSafe("orderDetailTAX") as decimal?,
                orderDetailTotal = GetValueSafe("orderDetailTotal") as decimal?,
                orderDetailCurrencyId = GetValueSafe("orderDetailCurrencyId") as int?,
                orderDetailCreatorId = GetValueSafe("orderDetailCreatorId") as int?,
                orderDetailCreationDate = GetValueSafe("orderDetailCreationDate") as DateTime?,
                orderDetailModificatorId = GetValueSafe("orderDetailModificatorId") as int?,
                orderDetailModificationDate = GetValueSafe("orderDetailModificationDate") as DateTime?,
                orderDetailStatusId = GetValueSafe("orderDetailStatusId") as bool?
            };
        }
    }
    }
