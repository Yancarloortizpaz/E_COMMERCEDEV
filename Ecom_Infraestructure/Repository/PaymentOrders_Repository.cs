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
    public class PaymentOrders_Repository : IPaymentOrders
    {
        private readonly DB_conection _conection;

        public PaymentOrders_Repository(DB_conection conection)
        {
            _conection = conection;
        }

        public async Task<IEnumerable<PaymentOrders_DTOS>> LISTAR_PAYMENTORDERS_ASYNC()
        {
            var list = new List<PaymentOrders_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_PaymentOrders_List]", con))
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

        public async Task<IEnumerable<PaymentOrders_DTOS>> FILTRAR_PAYMENTORDERS_ASYNC(int? userId, string searchTerm, int? statusId)
        {
            var list = new List<PaymentOrders_DTOS>();
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_PaymentOrders_Filter]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserId", userId ?? (object)DBNull.Value));
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

        public async Task<(int code, string message, int? templateId)> NUEVO_PAYMENTORDERS_ASYNC(
            int orderUserId,
            int orderDeliveryAddress,
            int orderPaymentMethodId,
            decimal? orderSubtotal,
            decimal? orderDiscount,
            decimal orderShipping,
            decimal? orderTAX,
            decimal? orderTotal,
            int? orderCurrencyId,
            int orderCreatorId,
            int orderStatusId)
        {
            try
            {
                using var con = _conection.CreateConnection();
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("[SQM_GENERAL].[sp_PaymentOrders_Create]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@orderUserId", orderUserId));
                    cmd.Parameters.Add(new SqlParameter("@orderDeliveryAddress", orderDeliveryAddress));
                    cmd.Parameters.Add(new SqlParameter("@orderPaymentMethodId", orderPaymentMethodId));
                    cmd.Parameters.Add(new SqlParameter("@orderSubtotal", orderSubtotal ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@orderDiscount", orderDiscount ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@orderShipping", orderShipping));
                    cmd.Parameters.Add(new SqlParameter("@orderTAX", orderTAX ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@orderTotal", orderTotal ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@orderCurrencyId", orderCurrencyId ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@orderCreatorId", orderCreatorId));
                    cmd.Parameters.Add(new SqlParameter("@orderStatusId", orderStatusId));

                    SqlParameter oCode = new SqlParameter("@o_code", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter oMessage = new SqlParameter("@o_message", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    SqlParameter oTemplate = new SqlParameter("@o_templateId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(oCode);
                    cmd.Parameters.Add(oMessage);
                    cmd.Parameters.Add(oTemplate);

                    
                    cmd.CommandTimeout = 60;

                    await cmd.ExecuteNonQueryAsync();

                    int code = oCode.Value != DBNull.Value ? (int)oCode.Value : 200;
                    string message = oMessage.Value != DBNull.Value ? oMessage.Value.ToString()! : "Checkout realizado con éxito. Orden e inventarios actualizados correctamente.";
                    int? templateId = oTemplate.Value != DBNull.Value ? (int?)oTemplate.Value : null;

                    return (code, message, templateId);
                }
            }
            catch (SqlException) { throw; }
            catch (Exception ex) { throw new Exception("Error de infraestructura al procesar el checkout de la orden de pago", ex); }
        }

        private PaymentOrders_DTOS MapearDTO(SqlDataReader dr)
        {
            return new PaymentOrders_DTOS
            {
                orderId = dr["orderId"] as int?,
                orderUserId = dr["userId"] as int?,
                orderDeliveryAddress = dr["deliveryAddressId"] as int?,
                orderPaymentMethodId = dr["paymentMethodId"] as int?,
                orderSubtotal = dr["subtotal"] as decimal?,
                orderDiscount = dr["discount"] as decimal?,
                orderShipping = dr["shipping"] as decimal?,
                orderTAX = dr["tax"] as decimal?,
                orderTotal = dr["total"] as decimal?,
                orderCurrencyId = dr["currencyId"] as int?,
                orderCreatorId = null, 
                orderCreationDate = dr["creationDate"] as DateTime?,
                orderModificatorId = null, 
                orderModificationDate = null, 
                orderStatusId = dr["statusId"] as int?
            };
        }
    }
}